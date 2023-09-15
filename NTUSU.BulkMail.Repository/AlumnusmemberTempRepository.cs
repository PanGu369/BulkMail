using NTUST.BulkMail.EntityFramework;
using NTUST.BulkMail.EntityFramework.Infrastructure;
using NTUST.BulkMail.EntityFramework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTUST.BulkMail.Repository
{
    public class AlumnusmemberTempRepository : GenericRepository<int, alumnusmember_temp>, IAlumnusmemberTempRepository
    {
        public AlumnusmemberTempRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
