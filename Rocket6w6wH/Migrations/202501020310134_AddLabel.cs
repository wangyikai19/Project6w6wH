namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLabel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Member", "MemberPictureUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Member", "MemberPictureUrl");
        }
    }
}
