using Core.Events;
using EntityFramework.Extensions;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Database.Repositories
{
    public abstract class Repository : IBaseRepository
    {
        private readonly DbContext _dbContext;
        private IEventPublisher _eventPublisher => ServiceLocator.Current.GetInstance<IEventPublisher>();

        protected Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbContext DbContext
        {
            get { return _dbContext; }
        }

        public DbSet<T> DbSet<T>() where T : class
        {
            return _dbContext.Set<T>();
        }

        public virtual int SaveChanges()
        {
            try
            {
                return _dbContext.SaveChanges();
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public void EnableAutoDetectChanges()
        {
            _dbContext.Configuration.AutoDetectChangesEnabled = true;
        }

        public void DisableAutoDetectChanges()
        {
            _dbContext.Configuration.AutoDetectChangesEnabled = false;
        }

        #region Gets

        public virtual IQueryable<T> GetAll<T>() where T : class
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public T Get<T>(params object[] id) where T : class
        {
            return GetById<T>(id);
        }

        public IQueryable<T> ExecuteSql<T>(string sqlStament) where T : class
        {
            return _dbContext.Database.SqlQuery<T>(sqlStament).AsQueryable();
        }

        public T Get<T>(Expression<Func<T, bool>> selector) where T : class
        {
            return First(selector);
        }

        public virtual T GetById<T>(params object[] id) where T : class
        {
            return _dbContext.Set<T>().Find(id);
        }

        public virtual IQueryable<T> GetByIds<T>(params object[] ids) where T : class
        {
            return ids.Select(id => GetById<T>(id))
                      .Where(x => x != null)
                      .AsQueryable();
        }

        public virtual T First<T>(Expression<Func<T, bool>> predicate = null) where T : class
        {
            return predicate == null
                       ? _dbContext.Set<T>().FirstOrDefault()
                       : _dbContext.Set<T>().FirstOrDefault(predicate);
        }

        public virtual int Count<T>(Expression<Func<T, bool>> expression = null) where T : class
        {
            return expression == null
                       ? _dbContext.Set<T>().Count()
                       : _dbContext.Set<T>().Count(expression);
        }

        public virtual IQueryable<T> Fetch<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return _dbContext.Set<T>().Where(expression);
        }

        public virtual bool Any<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return _dbContext.Set<T>().Any(expression);
        }

        public virtual bool All<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return _dbContext.Set<T>().All(expression);
        }

        #endregion

        #region Insert

        public virtual void Insert<T>(T entity) where T : class
        {
            try
            {
                ((dynamic)entity).CreatedDate = DateTimeOffset.Now;
                _eventPublisher.Publish(new EntityBeingInserted { Entity = entity });
            }
            catch
            {
            }

            var entityEntry = _dbContext.Entry(entity);

            if (entityEntry.State != EntityState.Detached)
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                _dbContext.Set<T>().Add(entity);
            }
        }

        #endregion

        #region Update

        public virtual void Update<T>(T entity) where T : class
        {
            try
            {
                ((dynamic)entity).ModifiedDate = DateTimeOffset.Now;
                _eventPublisher.Publish(new EntityBeingUpdated { Entity = entity });
            }
            catch
            {
                // entity does not have definition of ModifiedDate field, ignore error.
            }

            var entityEntry = _dbContext.Entry(entity);

            if (entityEntry.State == EntityState.Detached)
            {
                _dbContext.Set<T>().Attach(entity);
            }


            entityEntry.State = EntityState.Modified;
        }

        public virtual void Update<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> dispatch) where T : class
        {
            _dbContext.Set<T>().Where(predicate).Update(dispatch);
        }

        public virtual void Update<T>(Expression<Func<T, T>> dispatch) where T : class
        {
            _dbContext.Set<T>().Update(dispatch);
        }

        #endregion

        #region Insert or update

        public virtual void InsertOrUpdate<T>(Expression<Func<T, object>> identifierExpression, T entity) where T : class
        {
            _dbContext.Set<T>().AddOrUpdate(identifierExpression, entity);
        }

        public virtual void InsertOrUpdate<T>(Expression<Func<T, object>> identifierExpression, IEnumerable<T> entities) where T : class
        {
            var entitiesToInsert = entities.ToArray();

            _dbContext.Set<T>().AddOrUpdate(identifierExpression, entitiesToInsert);
        }

        #endregion

        #region Delete

        public virtual void DeleteByCondition<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            _dbContext.Set<T>().Where(predicate).Delete();
        }

        public virtual void Delete<T>(T entity) where T : class
        {
            try
            {
                ((dynamic)entity).IsDeleted = true;
                Update(entity);
            }
            catch
            {
                _dbContext.Set<T>().Remove(entity);
            }
        }

        public void HardDelete<T>(T entity) where T : class
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
            Delete(entity);
        }

        #endregion

        public void Dispose()
        {
            SaveChanges();
        }
    }
}