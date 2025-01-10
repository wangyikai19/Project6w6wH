namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNotify : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReplyId = c.Int(nullable: false),
                        ReplyUserId = c.Int(nullable: false),
                        CommentId = c.Int(nullable: false),
                        CommentUserId = c.Int(nullable: false),
                        Check = c.Int(nullable: false),
                        CreateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Replies", t => t.ReplyId, cascadeDelete: true)
                .Index(t => t.ReplyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifies", "ReplyId", "dbo.Replies");
            DropIndex("dbo.Notifies", new[] { "ReplyId" });
            DropTable("dbo.Notifies");
        }
    }
}
