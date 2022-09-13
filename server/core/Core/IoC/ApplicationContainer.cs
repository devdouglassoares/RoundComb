using Core.Logging;
using Core.StartUp;
using Core.Templating.Services;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;

// [assembly: PreApplicationStartMethod(typeof(ApplicationContainer), "LoadModuleAssembliesToCache")]
namespace Core.IoC
{
    public class ApplicationContainer
    {
        public static IEnumerable<Assembly> ApplicationAssemblies;
        private static bool _isInitialized;
        private static readonly ILogger Logger = Logging.Logger.GetLogger<ApplicationContainer>();

        public static void LoadModuleAssembliesToCache()
        {
            if (_isInitialized)
                return;

            PreLoadDeployedAssemblies();

            ApplicationAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                                             .FilterNotGacAssemblies()
                                             .ToArray();

            ApplicationAssemblies = ApplicationAssemblies.SelectMany(x => x.GetReferencedAssemblies())
                                                         .Select(x =>
                                                                 {
                                                                     try
                                                                     {
                                                                         return Assembly.Load(x);
                                                                     }
                                                                     catch
                                                                     {
                                                                         return null;
                                                                     }
                                                                 })
                                                         .Where(x => x != null)
                                                         .FilterNotGacAssemblies()
                                                         .Concat(ApplicationAssemblies)
                                                         .Distinct()
                                                         .ToArray();
            _isInitialized = true;
        }

        public static void CreateApplicationContainer<T>(IContainerConfig config, out T containerObject) where T : class
        {
            LoadModuleAssembliesToCache();
            config.ConfigureContainer(out containerObject);
            BootstrapApplication();
        }

        public static void CreateApplicationContainer(IContainerConfig config)
        {
            LoadModuleAssembliesToCache();
            config.ConfigureContainer();
            BootstrapApplication();
        }

        private static void BootstrapApplication()
        {
            try
            {
                var applicationBootstrapping = ServiceLocator.Current.GetInstance<IApplicationBootstrapping>();

                applicationBootstrapping?.StartUp();
            }
            catch (Exception exception)
            {
                Logger.Error("Error while running startup bootstrapping", exception);
            }

            try
            {
                var templatingRegistration = ServiceLocator.Current.GetInstance<ITemplateRegistration>();

                templatingRegistration?.RegisterTemplates();
            }
            catch (Exception exception)
            {
                Logger.Error("Error while running template registration", exception);
            }
        }


        private static IEnumerable<ModuleEntry> GetBinFolders()
        {
            var binFolders = new List<ModuleEntry>();

            binFolders.Add(new ModuleEntry
            {
                BinPath =
                                   HttpContext.Current != null
                                       ? HttpRuntime.BinDirectory
                                       : AppDomain.CurrentDomain.BaseDirectory,
                ModuleFolder = AppDomain.CurrentDomain.BaseDirectory,
                ModuleName = "MainModule"
            });

            var modulePaths = ConfigurationManager.AppSettings["ModulePaths"];
            if (!string.IsNullOrEmpty(modulePaths))
            {
                var paths = modulePaths.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                var basePath = HttpContext.Current != null ? HostingEnvironment.MapPath("~/") : AppDomain.CurrentDomain.BaseDirectory;

                LoadExtensionModuleBinFolders(basePath, paths, binFolders);
            }

            return binFolders;
        }

        private static readonly List<FileSystemWatcher> _fileSystemWatchers = new List<FileSystemWatcher>();

        private static void LoadExtensionModuleBinFolders(string basePath, string[] paths, List<ModuleEntry> toReturn)
        {
            foreach (var path in paths)
            {
                var moduleContainerFolder = Path.Combine(basePath, path);
                if (!Directory.Exists(moduleContainerFolder))
                    continue;

                var folders = Directory.GetDirectories(moduleContainerFolder);

                foreach (var folder in folders)
                {
                    var binPath = Path.Combine(folder, "bin");
                    if (Directory.Exists(binPath))
                    {
                        var moduleEntry = new ModuleEntry
                        {
                            BinPath = binPath,
                            ModuleFolder = folder,
                            ModuleName = new DirectoryInfo(folder).Name
                        };
                        toReturn.Add(moduleEntry);
                        UnloadShadowBinPath(moduleEntry);
                    }
                }
            }
        }

        private static void PreLoadDeployedAssemblies()
        {
            foreach (var path in GetBinFolders())
            {
                PreLoadAssembliesFromPath(path);
            }
        }

        private static void AddWatcher(string path)
        {
            var fileSystemWatcher = new FileSystemWatcher(path, "*.*");

            fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            fileSystemWatcher.Renamed += FileSystemWatcher_Changed;
            fileSystemWatcher.Deleted += FileSystemWatcher_Changed;
            fileSystemWatcher.Created += FileSystemWatcher_Changed;
            fileSystemWatcher.EnableRaisingEvents = true;

            lock (_fileSystemWatchers)
            {
                _fileSystemWatchers.Add(fileSystemWatcher);
            }
        }

        private static void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            lock (_fileSystemWatchers)
            {
                _fileSystemWatchers.Clear();
                var success = TryWriteBinFolder() || TryWriteWebConfig();

                if (!success)
                {
                    AppDomain.Unload(AppDomain.CurrentDomain);
                }
            }
        }

        private static void UnloadShadowBinPath(ModuleEntry moduleEntry)
        {
            if (Directory.Exists(moduleEntry.ShadowBinPath))
            {
                try
                {
                    Directory.Delete(moduleEntry.ShadowBinPath, true);
                }
                catch (Exception exception)
                {
                    Logger.Error($"Error while deleting {moduleEntry.ModuleName} shadow bin path", exception);
                }
            }
        }

        private static readonly List<FileInfo> LoadedAssemblyFiles = new List<FileInfo>();

        private static void PreLoadAssembliesFromPath(ModuleEntry moduleEntry)
        {
            var p = moduleEntry.BinPath;
            if (!p.EndsWith("bin"))
                p = Path.Combine(p, "bin");

            var binDirectory = Directory.Exists(p) ? new DirectoryInfo(p) : new DirectoryInfo(moduleEntry.BinPath);

            var files = binDirectory.GetFiles("*.dll", SearchOption.AllDirectories);

            var loadedAssemblies = files.Where(x => LoadedAssemblyFiles.Select(y => y.Name).Contains(x.Name));

            // delete all loaded assemblies, we only need the extension modules and not duplicated ones.
            foreach (var loadedAssembly in loadedAssemblies)
            {
                loadedAssembly.Delete();
            }

            files = files.Where(x => !LoadedAssemblyFiles.Select(y => y.Name).Contains(x.Name)).ToArray();

#if DEBUG
            if (moduleEntry.ModuleName != "MainModule")
            {
                AddWatcher(moduleEntry.BinPath);

                if (!Directory.Exists(moduleEntry.ShadowBinPath))
                    Directory.CreateDirectory(moduleEntry.ShadowBinPath);

                foreach (var fileInfo in files)
                {
                    fileInfo.CopyTo(Path.Combine(moduleEntry.ShadowBinPath, fileInfo.Name), true);
                }
                files = new DirectoryInfo(moduleEntry.ShadowBinPath).GetFiles("*.dll", SearchOption.AllDirectories);
            }
#endif
            LoadedAssemblyFiles.AddRange(files);

            foreach (var fi in files)
            {
                var fileName = fi.FullName;
                AssemblyName assemblyName;

                try
                {
                    assemblyName = AssemblyName.GetAssemblyName(fileName);
                }
                catch (Exception exception)
                {
                    Logger.Error($"Error while getting assembly name from {fileName}", exception);
                    continue;
                }

                if (!AppDomain.CurrentDomain.GetAssemblies().Any(assembly =>
                  AssemblyName.ReferenceMatchesDefinition(assemblyName, assembly.GetName())))
                {
                    var assembly = Assembly.Load(assemblyName);
                    try
                    {
                        BuildManager.AddReferencedAssembly(assembly);
                    }
                    catch (Exception exception)
                    {
                        AppDomain.CurrentDomain.Load(assemblyName);
                    }
                }
            }
        }

        private const string WebConfigPath = "~/web.config";
        private const string HostRestartPath = "~/bin/HostRestart";
        private static bool TryWriteWebConfig()
        {
            try
            {
                File.SetLastWriteTimeUtc(HostingEnvironment.MapPath(WebConfigPath), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool TryWriteBinFolder()
        {
            try
            {
                var binMarker = HostingEnvironment.MapPath(HostRestartPath);

                if (!string.IsNullOrEmpty(binMarker) && !Directory.Exists(binMarker))
                    Directory.CreateDirectory(binMarker);

                using (var stream = File.CreateText(Path.Combine(binMarker, "restart.txt")))
                {
                    stream.WriteLine($"Application restarted on '{DateTimeOffset.Now}'");
                    stream.Flush();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
