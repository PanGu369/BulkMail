﻿using NTUST.BulkMail.EntityFramework;
using NTUST.BulkMail.EntityFramework.Infrastructure;
using NTUST.BulkMail.EntityFramework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTUST.BulkMail.Repository
{
    public class StuMemberRepository : GenericRepository<int, stumember>, IStuMemberRepository
    {
        public StuMemberRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
