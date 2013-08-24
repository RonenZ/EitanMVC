namespace Eitan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SEO : DbMigration
    {
        public override void Up()
        {
            //DropIndex("dbo.Releases", new[] { "SeoId" });
            //DropIndex("dbo.News", new[] { "SeoId" });
            //DropIndex("dbo.Projects", new[] { "SeoId" });
            ////DropIndex("dbo.Pages", new[] { "SeoId" });
            //AddColumn("dbo.SEOs", "SEO_ID", c => c.Int(nullable: false, identity: true));
            AddForeignKey("dbo.Releases", "SeoId", "dbo.SEOs", "SEO_ID", cascadeDelete: true);
            AddForeignKey("dbo.News", "SeoId", "dbo.SEOs", "SEO_ID", cascadeDelete: true);
            AddForeignKey("dbo.Projects", "SeoId", "dbo.SEOs", "SEO_ID", cascadeDelete: true);
            AddForeignKey("dbo.Pages", "SeoId", "dbo.SEOs", "SEO_ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
        }
    }
}
