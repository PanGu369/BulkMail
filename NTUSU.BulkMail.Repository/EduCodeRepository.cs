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
    public class EduCodeRepository : GenericRepository<int, educode>, IEduCodeRepository
    {
        public EduCodeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
