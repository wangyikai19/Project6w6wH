namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class storecommentaddstar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreComments", "Stars", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StoreComments", "Stars");
        }
    }
}
