namespace Eitan.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class check2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "MainImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "MainImage");
        }
    }
}
