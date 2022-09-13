using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using Core.Database.Entities;
using Core.Database.Repositories;
using Core.ObjectMapping;

namespace Core.Database
{
    public abstract class BaseEntityService<TEntity, TDto> : BaseService<TEntity, TDto>
        where TEntity : BaseEntity, new()
        where TDto : class
    {
        protected BaseEntityService(IMappingService mappingService, IBaseRepository repository) : base(mappingService, repository)
        {
        }

        public override IQueryable<TEntity> GetAll()
        {
            return base.GetAll().Where(x => !x.IsDeleted);
        }
    }
}