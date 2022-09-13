using Core.Database.Repositories;
using Core.Events;
using Core.ObjectMapping;
using DocumentsManagement.Library.Dtos;
using DocumentsManagement.Library.Entities;
using DocumentsManagement.Library.Events;
using System;
using System.Linq;

namespace DocumentsManagement.Library.Services
{
    public abstract class BaseDocumentsService<TDto, TEntity> : IBaseDocumentsService<TDto, TEntity> where TDto : BaseDocumentDto where TEntity : BaseDocumentEntity
    {
        private readonly IBaseRepository _repository;

        private readonly IMappingService _mappingService;
        private readonly IEventPublisher _eventPublisher;

        protected BaseDocumentsService(IBaseRepository repository, IMappingService mappingService, IEventPublisher eventPublisher)
        {
            this._repository = repository;
            _mappingService = mappingService;
            _eventPublisher = eventPublisher;
        }

        public IQueryable<TDto> GetAll(long masterId)
        {
            var queryable = _repository.GetAll<TEntity>().Where(e => e.MasterId == masterId && !e.IsDeleted);
            return _mappingService.Project<TEntity, TDto>(queryable);
        }

        public TDto Get(long id)
        {
            var document = _repository.Get<TEntity>(id);

            if (document == null || document.IsDeleted)
            {
                return null;
            }

            var model = _mappingService.Map<TDto>(document);
            model.FileName = document.FileRecord.FileName;
            model.FileUrl = document.FileRecord.FileUrl;
            model.ModifiedBy = document.FileRecord.ModifiedBy;
            model.ModifiedDate = document.FileRecord.ModifiedDate;
            model.CreatedBy = document.FileRecord.CreatedBy;
            model.CreatedDate = document.FileRecord.CreatedDate;

            return model;
        }

        public TDto Create(TDto model)
        {
            var currentAuthoring = new CurrentDocumentAuthoring();
            _eventPublisher.Publish(currentAuthoring);

            var documentEntity = _mappingService.Map<TEntity>(model);
            documentEntity.MasterId = model.MasterId;
            documentEntity.Comment = model.Comment;
            documentEntity.ModifiedDate = DateTimeOffset.UtcNow;
            documentEntity.ModifiedBy = currentAuthoring.AuthorName;
            documentEntity.CreatedDate = DateTimeOffset.UtcNow;
            documentEntity.CreatedBy = currentAuthoring.AuthorName;

            _repository.Insert(documentEntity);
            _repository.SaveChanges();
            model.Id = documentEntity.Id;
            return model;
        }

        public void Delete(long id)
        {
            var currentAuthoring = new CurrentDocumentAuthoring();
            _eventPublisher.Publish(currentAuthoring);

            var document = _repository.Get<TEntity>(id);
            if (document == null)
            {
                return;
            }

            document.IsDeleted = true;
            document.ModifiedBy = currentAuthoring.AuthorName;
            document.ModifiedDate = DateTimeOffset.UtcNow;
            _repository.SaveChanges();
        }

        public void Update(long id, TDto model)
        {
            var currentAuthoring = new CurrentDocumentAuthoring();
            _eventPublisher.Publish(currentAuthoring);

            var document = _repository.Get<TEntity>(id);
            if (document == null)
            {
                return;
            }

            document.Comment = model.Comment;
            document.FileRecord.FileName = model.FileName;
            document.ModifiedBy = currentAuthoring.AuthorName;
            document.ModifiedDate = DateTimeOffset.UtcNow;
            _repository.Update(document);
            _repository.Update(document.FileRecord);
            _repository.SaveChanges();
        }
        public FileRecordDto SaveFileRecord(string fileName, string filePath)
        {
            var currentDocumentAuthoring = new CurrentDocumentAuthoring();
            _eventPublisher.Publish(currentDocumentAuthoring);

            var fileRecord = new FileRecord
            {
                FileName = fileName,
                FileUrl = filePath,
                CreatedDate = DateTimeOffset.Now,
                CreatedBy = currentDocumentAuthoring.AuthorName,
                ModifiedDate = DateTimeOffset.Now,
                ModifiedBy = currentDocumentAuthoring.AuthorName
            };
            _repository.Insert(fileRecord);
            _repository.SaveChanges();
            return _mappingService.Map<FileRecordDto>(fileRecord);
        }
    }
}