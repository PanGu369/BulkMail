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
    public class MailGroupRepository : GenericRepository<int, mailgroup>, IMailGroupRepository
    {
        public MailGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
