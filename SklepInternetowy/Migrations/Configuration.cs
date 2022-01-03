namespace SklepInternetowy.Migrations
{
    using SklepInternetowy.DAL;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<SklepInternetowy.DAL.KursyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "SklepInternetowy.DAL.KursyContext";
        }

        protected override void Seed(SklepInternetowy.DAL.KursyContext context)
        {
            KursyInitializer.SeedKursyData(context);
            KursyInitializer.SeedUzytkownicy(context);
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
