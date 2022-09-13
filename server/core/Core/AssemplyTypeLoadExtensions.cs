using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core
{
    public static class AssemplyTypeLoadExtensions
    {
        public static Type[] GetDefinedConcreteTypes(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException exception)
            {
                return exception.Types;
            }
        }
        /// <summary>
        /// Get all defined classes in provided list of assemblies, except abstract classes and interfaces and ignore classes
        /// </summary>
        /// <param name="assemblies">List of assemblies to get classes</param>
        /// <returns>Array of classes</returns>
        public static Type[] GetDefinedConcreteTypes(this IEnumerable<Assembly> assemblies)
        {
            var assembliesArray = assemblies as Assembly[] ?? assemblies.ToArray();

            var suppressDependencies = GetExcludedTypes(assembliesArray);

            return assembliesArray.SelectMany(x => x.GetDefinedConcreteTypes())
                             .Where(type => type != null && !type.IsInterface && !type.IsAbstract)
                             .Where(type => !suppressDependencies.Contains(type.FullName))
                             .ToArray();
        }

        public static IEnumerable<string> GetExcludedTypes(IEnumerable<Assembly> moduleEntries)
        {
            var excludedTypes = new HashSet<string>();

            var allTypes = moduleEntries.SelectMany(x => x.GetDefinedConcreteTypes())
                                        .Where(type => type != null && !type.IsInterface && !type.IsAbstract)
                                        .ToArray();

            foreach (var type in allTypes)
            {
                foreach (var replacedType in type.GetCustomAttributes<SuppressDependencyAttribute>(false))
                {
                    if (excludedTypes.Contains(replacedType.FullName))
                        continue;

                    excludedTypes.Add(replacedType.FullName);
                }
            }

            return excludedTypes;
        }
    }
}