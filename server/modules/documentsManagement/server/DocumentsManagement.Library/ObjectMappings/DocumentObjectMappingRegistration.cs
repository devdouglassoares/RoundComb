using Core.ObjectMapping;
using DocumentsManagement.Library.Dtos;
using DocumentsManagement.Library.Entities;

namespace DocumentsManagement.Library.ObjectMappings
{
    public class DocumentObjectMappingRegistration : IObjectMappingRegistration
    {
        public void ConfigureMapping(IMappingService map)
        {
            map.ConfigureMapping<FileRecordDto, FileRecord>();
            map.ConfigureMapping<FileRecord, FileRecordDto>();
        }
    }
}