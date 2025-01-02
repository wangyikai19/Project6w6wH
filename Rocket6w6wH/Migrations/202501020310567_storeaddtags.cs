namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class storeaddtags : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stores", "StoreTags", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stores", "StoreTags");
        }
    }
}
