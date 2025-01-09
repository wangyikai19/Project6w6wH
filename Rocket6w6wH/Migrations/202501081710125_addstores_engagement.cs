namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addstores_engagement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stores", "Engagement", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stores", "Engagement");
        }
    }
}
