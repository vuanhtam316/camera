﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyWeb.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities1 : DbContext
    {
        public Entities1()
            : base("name=Entities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<TBL_CAMERA> TBL_CAMERA { get; set; }
        public DbSet<TBL_CAMERA_FUNCTION> TBL_CAMERA_FUNCTION { get; set; }
        public DbSet<TBL_CAMERA_STATUS> TBL_CAMERA_STATUS { get; set; }
        public DbSet<TBL_FUNCTION> TBL_FUNCTION { get; set; }
        public DbSet<TBL_HISTORY_IMAGES> TBL_HISTORY_IMAGES { get; set; }
        public DbSet<TBL_HISTORY_VIDEO> TBL_HISTORY_VIDEO { get; set; }
        public DbSet<TBL_PERMISSTION> TBL_PERMISSTION { get; set; }
        public DbSet<TBL_PROFILE_USER> TBL_PROFILE_USER { get; set; }
        public DbSet<TBL_ROLE> TBL_ROLE { get; set; }
        public DbSet<TBL_TOKEN> TBL_TOKEN { get; set; }
        public DbSet<TBL_USER> TBL_USER { get; set; }
        public DbSet<TBL_USER_ROLE> TBL_USER_ROLE { get; set; }
        public DbSet<TLB_USER_HISTORY> TLB_USER_HISTORY { get; set; }
        public DbSet<TBL_FEEDBACK> TBL_FEEDBACK { get; set; }
        public DbSet<TBL_UPDATE_STATUS> TBL_UPDATE_STATUS { get; set; }
    }
}