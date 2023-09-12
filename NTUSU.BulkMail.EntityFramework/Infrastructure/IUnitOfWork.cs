using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTUST.BulkMail.EntityFramework.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext DbContext { get; }
        bool LazyLoadingEnabled { get; set; }
        bool AutoDetectChangesEnabled { get; set; }
        int Commit();
        Task<int> CommitAsync();
    }
}
