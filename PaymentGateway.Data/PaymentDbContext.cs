using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.EntityTypeConfiguration;
using PaymentGateway.Models;
using System;
using System.Collections.Generic;

namespace PaymentGateway.Data
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        {

        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ServiceXTransaction> ServiceXTransaction { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Service>().HasKey(x => x.Id);
            modelBuilder.Entity<Service>().Property(x => x.Id);//.HasColumnName("IdUlMeuSpecial");

            modelBuilder.Entity<ServiceXTransaction>().HasKey(x => new { x.ServiceIdList.IdService, x.IdTransaction });

            modelBuilder.ApplyConfiguration(new PersonConfiguration());
        }

    }
}
