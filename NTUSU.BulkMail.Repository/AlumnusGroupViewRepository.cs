using NTUST.BulkMail.EntityFramework.Infrastructure;
using NTUST.BulkMail.EntityFramework.Interface;
using NTUST.BulkMail.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTUST.BulkMail.Repository
{
    public class AlumnusGroupViewRepository : GenericRepository<int, AlumnusGroupView>, IAlumnusGroupViewRepository
    {
        public AlumnusGroupViewRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
