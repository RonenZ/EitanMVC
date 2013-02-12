namespace Eitan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update12022013 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Songs", "SoundCloudID", c => c.String());
            AlterColumn("dbo.Releases", "CategoryNum", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Releases", "CategoryNum", c => c.Int(nullable: false));
            DropColumn("dbo.Songs", "SoundCloudID");
        }
    }
}
