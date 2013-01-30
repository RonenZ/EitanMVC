namespace Eitan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PageId = c.Int(nullable: false),
                        Source = c.String(),
                        Date_Creation = c.DateTime(nullable: false),
                        Title = c.String(),
                        isDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Pages", t => t.PageId)
                .Index(t => t.PageId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Pictures", new[] { "PageId" });
            DropForeignKey("dbo.Pictures", "PageId", "dbo.Pages");
            DropTable("dbo.Pictures");
        }
    }
}
