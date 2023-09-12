using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTUST.BulkMail.EntityFramework.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory _dbFactory;
        private DbContext _dbContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public DbContext DbContext => _dbContext ?? (_dbContext = _dbFactory.Init());
        public int Commit()
        {
            try
            {
                DbContext.Configuration.AutoDetectChangesEnabled = false;
                return DbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                //來源： https://stackoverflow.com/questions/15820505/dbentityvalidationexception-how-can-i-easily-tell-what-caused-the-error

                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

                //return 0;
            }
        }
        public Task<int> CommitAsync()
        {
            return DbContext.SaveChangesAsync();
        }

        public bool LazyLoadingEnabled
        {
            get { return DbContext.Configuration.LazyLoadingEnabled; }
            set { DbContext.Configuration.LazyLoadingEnabled = value; }
        }
        public bool AutoDetectChangesEnabled
        {
            get { return DbContext.Configuration.AutoDetectChangesEnabled; }
            set { DbContext.Configuration.AutoDetectChangesEnabled = false; }
        }
        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
