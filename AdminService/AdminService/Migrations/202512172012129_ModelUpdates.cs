namespace AdminService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelUpdates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Notifications", new[] { "CustomerId" });
            DropTable("dbo.Notifications");
        }
    }
}
