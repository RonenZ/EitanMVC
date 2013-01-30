namespace Eitan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PicturePage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SEOs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ogTitle = c.String(),
                        ogImage = c.String(),
                        ogDescription = c.String(),
                        metaTitle = c.String(),
                        metaImage = c.String(),
                        metaDescription = c.String(),
                        metaKeywords = c.String(),
                        Type = c.Int(nullable: false),
                        isDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Pages", "SEO_ID", c => c.Int(nullable: false));
            AddColumn("dbo.Pages", "SEO_ID1", c => c.Int());
            AddColumn("dbo.Pictures", "PictureType", c => c.Int(nullable: false));
            AddForeignKey("dbo.Pages", "SEO_ID1", "dbo.SEOs", "ID");
            CreateIndex("dbo.Pages", "SEO_ID1");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Pages", new[] { "SEO_ID1" });
            DropForeignKey("dbo.Pages", "SEO_ID1", "dbo.SEOs");
            DropColumn("dbo.Pictures", "PictureType");
            DropColumn("dbo.Pages", "SEO_ID1");
            DropColumn("dbo.Pages", "SEO_ID");
            DropTable("dbo.SEOs");
        }
    }
}
