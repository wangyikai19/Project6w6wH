namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLabel02 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreComments", "Label", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StoreComments", "Label");
        }
    }
}
