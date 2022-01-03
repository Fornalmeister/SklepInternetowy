using Microsoft.AspNet.Identity.EntityFramework;
using SklepInternetowy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SklepInternetowy.DAL
{
    public class KursyContext : IdentityDbContext<ApplicationUser>
    {

        public KursyContext() : base("KursyContext")
        {

        }

        static KursyContext()
        {
            Database.SetInitializer<KursyContext>(new KursyInitializer());
        }

        public static KursyContext Create()
        {
            return new KursyContext();
        }

        public DbSet<Kurs> Kursy { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderPosition> OrderPosition { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //usuwamy końcówki es (liczbe mnoga)
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}