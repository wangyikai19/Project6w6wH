namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SDA : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentId = c.Int(nullable: false),
                        MemberId = c.Int(nullable: false),
                        message = c.String(),
                        CreateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StoreComments", t => t.CommentId, cascadeDelete: true)
                .ForeignKey("dbo.Member", t => t.MemberId)
                .Index(t => t.CommentId)
                .Index(t => t.MemberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentMessages", "MemberId", "dbo.Member");
            DropForeignKey("dbo.CommentMessages", "CommentId", "dbo.StoreComments");
            DropIndex("dbo.CommentMessages", new[] { "MemberId" });
            DropIndex("dbo.CommentMessages", new[] { "CommentId" });
            DropTable("dbo.CommentMessages");
        }
    }
}
