namespace Eitan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class check5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Releases", "GenreID", c => c.Int(nullable: false));
            CreateIndex("dbo.Releases", "GenreID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Releases", new[] { "GenreID" });
            DropColumn("dbo.Releases", "GenreID");
        }
    }
}
