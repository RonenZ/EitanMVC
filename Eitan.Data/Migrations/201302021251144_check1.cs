namespace Eitan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class check1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date_Creation = c.DateTime(nullable: false),
                        Title = c.String(),
                        isDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddForeignKey("dbo.Projects", "ClientID", "dbo.Clients", "ID", cascadeDelete: true);
            CreateIndex("dbo.Projects", "ClientID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Projects", new[] { "ClientID" });
            DropForeignKey("dbo.Projects", "ClientID", "dbo.Clients");
            DropTable("dbo.Clients");
        }
    }
}
