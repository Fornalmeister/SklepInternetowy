namespace SklepInternetowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Elmah : DbMigration
    {
        public override void Up()
        {
            SqlFile("../Migrations/Elmah-1.2-db-SQLServer.sql");
        }
        
        public override void Down()
        {
        }
    }
}
