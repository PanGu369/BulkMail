﻿using NTUST.BulkMail.EntityFramework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTUST.BulkMail.EntityFramework.Interface
{
    public interface IAlumnusGroupViewRepository : IRepository<int, AlumnusGroupView>
    {
    }
}
