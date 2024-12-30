namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatestore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stores", "StoreUrl", c => c.String());
            AddColumn("dbo.Stores", "StoreGoogleId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stores", "StoreGoogleId");
            DropColumn("dbo.Stores", "StoreUrl");
        }
    }
}
