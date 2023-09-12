using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Odbc;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NTUST.BulkMail.EntityFramework.Infrastructure
{
    public abstract class GenericRepository<TKey, TEntity> : IRepository<TKey, TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        private DbContext _dbContext;

        protected GenericRepository(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            _dbSet = DbContext.Set<TEntity>();
        }

        protected IDbFactory DbFactory { get; }

        protected DbContext DbContext => _dbContext ?? (_dbContext = DbFactory.Init());

        #region Implementation

        public TEntity Add(TEntity entity)
        {
            DbContext.Configuration.AutoDetectChangesEnabled = false;
            return _dbSet.Add(entity);
        }
        public void AddRange(List<TEntity> entitys)
        {

            _dbSet.AddRange(entitys);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }


        public TEntity Delete(TEntity entity)
        {
            return _dbSet.Remove(entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var objects = _dbSet.Where(predicate).AsEnumerable();
            foreach (var obj in objects)
                _dbSet.Remove(obj);
        }

        public TEntity GetById(TKey id)
        {
            return _dbSet.Find(id);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate, params string[] navigationProperties)
        {
            var query = _dbSet.AsQueryable();

            query = navigationProperties.Aggregate(query, (current, navigationProperty) => RepositoryIQueryableExtensions.Include(current, navigationProperty));

            return query.Where(predicate).FirstOrDefault();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<TEntity> GetAll(params string[] navigationProperties)
        {
            var query = _dbSet.AsQueryable();

            return navigationProperties.Aggregate(query, (current, navigationProperty) => RepositoryIQueryableExtensions.Include(current, navigationProperty));
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }


        public IQueryable<TEntity> GetAllNoTraking()
        {
            return _dbSet.AsNoTracking();
        }



        #endregion
    }
}
