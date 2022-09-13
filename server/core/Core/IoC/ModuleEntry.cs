using System.IO;

namespace Core.IoC
{
    public class ModuleEntry
    {
        public string ModuleName { get; set; }

        public string ModuleFolder { get; set; }

        public string ShadowBinPath => Path.Combine(ModuleFolder, "_shadowBin");

        public string BinPath { get; set; }
    }
}