using Core.Database;
using Core.ObjectMapping;
using ProductManagement.Core.Dto;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Repositories;
using ProductManagement.Core.Services;
using System;
using System.Linq;

namespace ProductManagement.Services
{
    public class TagService : BaseService<Tag, TagDto>, ITagService
    {
        private const string TagExistErrorMessage = "Another tag with the same name already exists";

        public TagService(IMappingService mappingService, IRepository repository) : base(mappingService, repository)
        {
        }

        public override IQueryable<Tag> GetAll()
        {
            return base.GetAll().Where(x => !x.IsDeleted);
        }

        public override Tag PrepareForInserting(Tag entity, TagDto model)
        {
            var tagexist = Repository.Any<Tag>(tag => tag.Name == model.Name.ToLower());
            if (tagexist)
            {
                throw new InvalidOperationException(TagExistErrorMessage);
            }

            entity = base.PrepareForInserting(entity, model);

            entity.Name = entity.Name.ToLowerInvariant();

            return entity;
        }

        public override Tag PrepareForUpdating(Tag entity, TagDto model)
        {
            var tagexist = Repository.Any<Tag>(tag => tag.Name == model.Name.ToLower() && tag.Id != entity.Id);
            if (tagexist)
                throw new InvalidOperationException(TagExistErrorMessage);

            entity = base.PrepareForUpdating(entity, model);

            entity.Name = entity.Name.ToLowerInvariant();

            return entity;
        }

        public override void Delete(params object[] keys)
        {
            var entity = GetEntity(keys);
            Repository.HardDelete(entity);
            Repository.SaveChanges();
        }
    }
}