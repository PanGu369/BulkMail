﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace NTUST.BulkMail.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class mailEntities : DbContext
    {
        public mailEntities()
            : base("name=mailEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<staffmember> staffmember { get; set; }
        public virtual DbSet<staffmember_temp> staffmember_temp { get; set; }
        public virtual DbSet<educode> educode { get; set; }
        public virtual DbSet<educode_temp> educode_temp { get; set; }
        public virtual DbSet<stumember> stumember { get; set; }
        public virtual DbSet<stumember_temp> stumember_temp { get; set; }
        public virtual DbSet<alumnusmember> alumnusmember { get; set; }
        public virtual DbSet<alumnusmember_temp> alumnusmember_temp { get; set; }
        public virtual DbSet<AlumnusGroupView> AlumnusGroupView { get; set; }
        public virtual DbSet<AlumCollegePeriodDepartmentYear> AlumCollegePeriodDepartmentYear { get; set; }
    }
}
