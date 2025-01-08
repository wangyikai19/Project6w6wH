namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReplynewcolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Replies", "ReplyOnlyCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Replies", "ReplyOnlyCode");
        }
    }
}
