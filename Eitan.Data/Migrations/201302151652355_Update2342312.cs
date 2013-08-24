namespace Eitan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update2342312 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SEOs", "bla", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SEOs", "bla");
        }
    }
}
