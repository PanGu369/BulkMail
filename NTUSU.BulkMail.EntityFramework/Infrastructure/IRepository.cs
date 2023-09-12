using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NTUST.BulkMail.EntityFramework.Infrastructure
{
    public interface IRepository<in TKey, TEntity>
    {
        // Marks an entity as new
        TEntity Add(TEntity entity);
        void AddRange(List<TEntity> entitys);
        // Marks an entity as modified
        void Update(TEntity entity);
        // Marks an entity to be removed
        TEntity Delete(TEntity entity);
        void Delete(Expression<Func<TEntity, bool>> predicate);

        // Get an entity by int id
        TEntity GetById(TKey id);
        // Get an entity using delegate
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        TEntity Get(Expression<Func<TEntity, bool>> predicate, params string[] navigationProperties);

        // Gets all entities of type T
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAllNoTraking();
        IQueryable<TEntity> GetAll(params string[] navigationProperties);
        // Gets entities using delegate
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
    }
}
