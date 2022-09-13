using System.Linq;
using DocumentsManagement.Library.Dtos;
using DocumentsManagement.Library.Entities;
using IDependency = Core.IDependency;

namespace DocumentsManagement.Library.Services
{
    public interface IBaseDocumentsService<TDto, TEntity> where TDto : BaseDocumentDto where TEntity : BaseDocumentEntity
    {
        IQueryable<TDto> GetAll(long masterId);
        TDto Get(long id);
        TDto Create(TDto model);
        void Delete(long id);
        void Update(long id, TDto model);

        FileRecordDto SaveFileRecord(string fileName, string filePath);

    }
}