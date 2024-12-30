namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MemberAddName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Member", "Name", c => c.String());
            CreateIndex("dbo.StoreComments", "MemberId");
            AddForeignKey("dbo.StoreComments", "MemberId", "dbo.Member", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreComments", "MemberId", "dbo.Member");
            DropIndex("dbo.StoreComments", new[] { "MemberId" });
            DropColumn("dbo.Member", "Name");
        }
    }
}
