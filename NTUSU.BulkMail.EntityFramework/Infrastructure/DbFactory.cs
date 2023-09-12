using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTUST.BulkMail.EntityFramework.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private mailEntities _dbContext;

        public DbContext Init()
        {
            return _dbContext ?? (_dbContext = new mailEntities());
        }

        protected override void DisposeCore()
        {
            _dbContext?.Dispose();
        }
    }
}
