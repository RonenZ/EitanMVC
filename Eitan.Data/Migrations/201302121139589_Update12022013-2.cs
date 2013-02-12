namespace Eitan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update120220132 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Songs", "isSoundCloudSet", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Songs", "isSoundCloudSet");
        }
    }
}
