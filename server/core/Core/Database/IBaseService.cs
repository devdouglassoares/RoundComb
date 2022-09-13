using Core.Database.Entities;
using Core.Database.Repositories;
using Core.Events;
using Core.Exceptions;
using Core.Logging;
using Core.ObjectMapping;
using Core.UI;
using Core.UI.DataTablesExtensions;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Database
{
    public interface IBaseService<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        IQueryable<TEntity> GetAll();

        TEntity CloneEntity(TEntity entityToClone);

        IEnumerable<TDto> GetAllDtos();

        IQueryable<TEntity> Fetch(Expression<Func<TEntity, bool>> expression);

	    IEnumerable<TDto> FetchDto(Expression<Func<TEntity, bool>> expression);

        DataTablesResponse GetDataTableResponse(DefaultDataTablesRequest requestModel);

        TEntity First(Expression<Func<TEntity, bool>> predicate);

        TEntity GetEntity(params object[] ids);

        TDto GetDto(params object[] ids);

        TEntity Create(TDto model);

        TEntity Update(TDto model, params object[] keys);

        void Delete(params object[] keys);

	    void HardDelete(params object[] keys);


		void InsertOrUpdate(TEntity entity, Expression<Func<TEntity, object>> identifierSelector);

        bool Any(Expression<Func<TEntity, bool>> expression);

        bool All(Expression<Func<TEntity, bool>> expression);

        IEnumerable<TDto> ToDtos(IEnumerable<TEntity> entities, bool wireup = true);

        IQueryable<TDto> ToQueryableDtos(IQueryable<TEntity> entities, bool wireup = true);

        TDto ToDto(TEntity entity);
    }

    public abstract class BaseService<TEntity, TDto> : IBaseService<TEntity, TDto>
        where TEntity : class, new()
        where TDto : class
    {
        protected readonly IMappingService MappingService;
        protected readonly IBaseRepository Repository;

        protected IDataTableService DataTableService => ServiceLocator.Current.GetInstance<IDataTableService>();

        protected IEventPublisher EventPublisher => ServiceLocator.Current.GetInstance<IEventPublisher>();

        protected readonly ILogger Logger;

        protected BaseService(IMappingService mappingService, IBaseRepository repository)
        {
            Logger = Logging.Logger.GetLogger(this.GetType());
            MappingService = mappingService;
            Repository = repository;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return Repository.GetAll<TEntity>();
        }

        public virtual IQueryable<TT> GetAll<TT>() where TT : BaseEntity
        {
            return Repository.GetAll<TT>().Where(x => !x.IsDeleted);
        }

        public TEntity CloneEntity(TEntity entityToClone)
        {
            Repository.DbContext.Entry(entityToClone).State = EntityState.Detached;

            return entityToClone;
        }

        public virtual IEnumerable<TDto> GetAllDtos()
        {
            return MappingService.Map<IEnumerable<TDto>>(GetAll());
        }

        public virtual IQueryable<TEntity> Fetch(Expression<Func<TEntity, bool>> expression)
        {
            return Repository.Fetch(expression);
        }

	    public IEnumerable<TDto> FetchDto(Expression<Func<TEntity, bool>> expression)
		{
			return MappingService.Map<IEnumerable<TDto>>(Repository.Fetch(expression).ToArray());
		}

	    public virtual DataTablesResponse GetDataTableResponse(DefaultDataTablesRequest requestModel)
        {
            var result = GetAll();

            var dto = ToQueryableDtos(result);

            dto = FilterForDataTables(dto, requestModel);

            try
            {
                return DataTableService.GetResponse(dto, requestModel);
            }
            catch (NotSupportedException exception)
            {
                Logger.Error(exception);
                dto = MappingService.Map<IEnumerable<TDto>>(result).AsQueryable();
                return DataTableService.GetResponse(dto, requestModel);
            }
        }

        public virtual IQueryable<TDto> FilterForDataTables<TDatatableRequestModel>(IQueryable<TDto> dtos, TDatatableRequestModel requestModel) where TDatatableRequestModel : DefaultDataTablesRequest
        {
            return dtos;
        }

        public virtual TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.First(predicate);
        }

        public virtual TEntity GetEntity(params object[] ids)
        {
            var entity = Repository.Get<TEntity>(ids);

            if (entity == null)
                throw new BaseNotFoundException<TEntity>();

            return entity;
        }

        public virtual TDto GetDto(params object[] ids)
        {
            var entity = GetEntity(ids);
            var dto = ToDto(entity);

            EventPublisher.Publish(new EntityDtoWireUp<TEntity, TDto> { Entity = entity, Dto = dto });

            return dto;
        }

        public virtual TEntity Create(TDto model)
        {
            var entity = PrepareForInserting(default(TEntity), model);

            EventPublisher.Publish(new EntityCreating<TEntity, TDto> { Entity = entity, Dto = model });

            Repository.Insert(entity);
            Repository.SaveChanges();

            EventPublisher.Publish(new EntityCreated<TEntity, TDto> { Entity = entity, Dto = model });

            return entity;
        }

        public virtual TEntity PrepareForInserting(TEntity entity, TDto model)
        {
            if (entity == null)
            {
                entity = Activator.CreateInstance<TEntity>();
            }

            MappingService.Map(model, entity);
            return entity;
        }

        public virtual TEntity Update(TDto model, params object[] keys)
        {
            var entity = GetEntity(keys);

            EventPublisher.Publish(new EntityUpdating<TEntity, TDto> { Entity = entity, Dto = model });
            entity = PrepareForUpdating(entity, model);

            Repository.Update(entity);
            //Repository.SaveChanges();
            EventPublisher.Publish(new EntityUpdated<TEntity, TDto> { Entity = entity, Dto = model });

            return entity;
        }

        public virtual TEntity PrepareForUpdating(TEntity entity, TDto model)
        {
            MappingService.Map(model, entity);
            return entity;
        }

        public virtual void Delete(params object[] keys)
        {
            var entity = GetEntity(keys);

            Repository.Delete(entity);
            Repository.SaveChanges();

            EventPublisher.Publish(new EntityDeleted<TEntity> { Entity = entity });
        }

	    public void HardDelete(params object[] keys)
		{
			var entity = GetEntity(keys);

			Repository.HardDelete(entity);
			Repository.SaveChanges();

			EventPublisher.Publish(new EntityDeleted<TEntity> { Entity = entity });
		}

	    public void InsertOrUpdate(TEntity entity, Expression<Func<TEntity, object>> identifierSelector)
        {
            Repository.InsertOrUpdate(identifierSelector, entity);
        }

        public bool Any(Expression<Func<TEntity, bool>> expression)
        {
            return Repository.Any(expression);
        }

        public bool All(Expression<Func<TEntity, bool>> expression)
        {
            return Repository.All(expression);
        }

        public virtual IEnumerable<TDto> ToDtos(IEnumerable<TEntity> entities, bool wireup = true)
        {
            var enumerable = MappingService.Map<IEnumerable<TDto>>(entities);

            if (wireup)
                EventPublisher.Publish(new EntitiesDtoWireUp<TEntity, TDto> { Entity = entities, Dto = enumerable });

            return enumerable;
        }

        public virtual IQueryable<TDto> ToQueryableDtos(IQueryable<TEntity> entities, bool wireup = true)
        {
            var enumerable = MappingService.Map<IEnumerable<TDto>>(entities);

            if (wireup)
                EventPublisher.Publish(new EntitiesDtoWireUp<TEntity, TDto> { Entity = entities, Dto = enumerable });

            return enumerable.AsQueryable();
        }

        public virtual TDto ToDto(TEntity entity)
        {
            return MappingService.Map<TDto>(entity);
        }
    }
}