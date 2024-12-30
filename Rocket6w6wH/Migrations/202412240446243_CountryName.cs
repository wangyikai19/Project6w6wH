namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CountryName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cities", "CountyName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cities", "CountyName");
        }
    }
}
