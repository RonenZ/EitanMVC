namespace Eitan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SEO2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Releases", "SeoId", c => c.Int());
            AlterColumn("dbo.News", "SeoId", c => c.Int());
            AlterColumn("dbo.Projects", "SeoId", c => c.Int());
            AlterColumn("dbo.Pages", "SeoId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pages", "SeoId", c => c.Int(nullable: false));
            AlterColumn("dbo.Projects", "SeoId", c => c.Int(nullable: false));
            AlterColumn("dbo.News", "SeoId", c => c.Int(nullable: false));
            AlterColumn("dbo.Releases", "SeoId", c => c.Int(nullable: false));
        }
    }
}
