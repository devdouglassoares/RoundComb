using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Database.Repositories
{
    public interface IBaseRepository : IDisposable
    {
        DbContext DbContext { get; }

        DbSet<T> DbSet<T>() where T : class;

        int SaveChanges();
        IQueryable<T> GetAll<T>() where T : class;

        T Get<T>(params object[] id) where T : class;

        IQueryable<T> ExecuteSql<T>(string sqlStament) where T : class;

        T Get<T>(Expression<Func<T, bool>> selector) where T : class;

        T GetById<T>(params object[] id) where T : class;

        IQueryable<T> GetByIds<T>(params object[] ids) where T : class;

        T First<T>(Expression<Func<T, bool>> predicate = null) where T : class;

        int Count<T>(Expression<Func<T, bool>> expression = null) where T : class;
        IQueryable<T> Fetch<T>(Expression<Func<T, bool>> expression) where T : class;
        bool Any<T>(Expression<Func<T, bool>> expression) where T : class;
        bool All<T>(Expression<Func<T, bool>> expression) where T : class;
        void Insert<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Update<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> dispatch) where T : class;
        void Update<T>(Expression<Func<T, T>> dispatch) where T : class;
        void InsertOrUpdate<T>(Expression<Func<T, object>> identifierExpression, T entity) where T : class;
        void InsertOrUpdate<T>(Expression<Func<T, object>> identifierExpression, IEnumerable<T> entities) where T : class;
        void DeleteByCondition<T>(Expression<Func<T, bool>> predicate) where T : class;
        void Delete<T>(T entity) where T : class;
        void HardDelete<T>(T entity) where T : class;

        [Obsolete("Use Delete method instead")]
        void Remove<T>(T entity) where T : class;

        void EnableAutoDetectChanges();

        void DisableAutoDetectChanges();
    }
}