﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RouteOptimization
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Optimization_RWEntities : DbContext
    {
        public Optimization_RWEntities()
            : base("name=Optimization_RWEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<channel> channels { get; set; }
        public virtual DbSet<delivery> deliveries { get; set; }
        public virtual DbSet<driver> drivers { get; set; }
        public virtual DbSet<hub> hubs { get; set; }
        public virtual DbSet<order> orders { get; set; }
        public virtual DbSet<payment> payments { get; set; }
        public virtual DbSet<store> stores { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    }
}
