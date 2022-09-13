using Core;

namespace ProductManagement.Core.Services
{
    public interface IPropertyImportService : IDependency
    {
        void ImportPropertyFromCsv(string csvSeparatedInput);
    }
}