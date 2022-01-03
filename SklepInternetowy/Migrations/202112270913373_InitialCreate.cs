namespace SklepInternetowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        NameCategory = c.String(nullable: false, maxLength: 100),
                        DescriptionCategory = c.String(nullable: false),
                        NameImg = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Kurs",
                c => new
                    {
                        KursId = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Author = c.String(nullable: false, maxLength: 100),
                        DateAdded = c.DateTime(nullable: false),
                        NameImg = c.String(maxLength: 100),
                        Description = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Bestseller = c.Boolean(nullable: false),
                        Hidden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.KursId)
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Street = c.String(nullable: false, maxLength: 100),
                        City = c.String(nullable: false, maxLength: 100),
                        Postmal = c.String(nullable: false, maxLength: 6),
                        Phone = c.String(),
                        Email = c.String(),
                        Comment = c.String(),
                        DateAdd = c.DateTime(nullable: false),
                        OrderStatus = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrderId);
            
            CreateTable(
                "dbo.OrderPosition",
                c => new
                    {
                        OrderPositionId = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        KursId = c.Int(nullable: false),
                        quantity = c.Int(nullable: false),
                        OrderCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.OrderPositionId)
                .ForeignKey("dbo.Kurs", t => t.KursId, cascadeDelete: true)
                .ForeignKey("dbo.Order", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.KursId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderPosition", "OrderId", "dbo.Order");
            DropForeignKey("dbo.OrderPosition", "KursId", "dbo.Kurs");
            DropForeignKey("dbo.Kurs", "CategoryId", "dbo.Category");
            DropIndex("dbo.OrderPosition", new[] { "KursId" });
            DropIndex("dbo.OrderPosition", new[] { "OrderId" });
            DropIndex("dbo.Kurs", new[] { "CategoryId" });
            DropTable("dbo.OrderPosition");
            DropTable("dbo.Order");
            DropTable("dbo.Kurs");
            DropTable("dbo.Category");
        }
    }
}
