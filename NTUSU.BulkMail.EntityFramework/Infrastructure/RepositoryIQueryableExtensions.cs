using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTUST.BulkMail.EntityFramework.Infrastructure
{
    public static class RepositoryIQueryableExtensions
    {
        public static IQueryable<T> Include<T>(this IQueryable<T> source, string path)
        {
            var objectQuery = source as ObjectQuery<T>;
            return objectQuery != null ? objectQuery.Include(path) : source;
        }
    }
}
