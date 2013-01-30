namespace Eitan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UPDATE6 : DbMigration
    {
        public override void Up()
        {
            
            
            AddColumn("dbo.Releases", "SeoId", c => c.Int(nullable: false));
            AddColumn("dbo.News", "SeoId", c => c.Int(nullable: false));
            AddColumn("dbo.Projects", "SeoId", c => c.Int(nullable: false));
            AddColumn("dbo.Pages", "SeoId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropIndex("dbo.SEOs", new[] { "ID" });
            DropForeignKey("dbo.SEOs", "ID", "dbo.Pages");
            DropForeignKey("dbo.SEOs", "ID", "dbo.Projects");
            DropForeignKey("dbo.SEOs", "ID", "dbo.News");
            DropForeignKey("dbo.SEOs", "ID", "dbo.Releases");
            DropColumn("dbo.Pages", "SeoId");
            DropColumn("dbo.Projects", "SeoId");
            DropColumn("dbo.News", "SeoId");
            DropColumn("dbo.Releases", "SeoId");
        }
    }
}
