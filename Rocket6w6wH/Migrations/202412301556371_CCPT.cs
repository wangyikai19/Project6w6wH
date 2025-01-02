namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CCPT : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentPictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentId = c.Int(nullable: false),
                        PictureUrl = c.String(),
                        CreateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StoreComments", t => t.CommentId, cascadeDelete: true)
                .Index(t => t.CommentId);
            
            AddColumn("dbo.StoreComments", "Stars", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentPictures", "CommentId", "dbo.StoreComments");
            DropIndex("dbo.CommentPictures", new[] { "CommentId" });
            DropColumn("dbo.StoreComments", "Stars");
            DropTable("dbo.CommentPictures");
        }
    }
}
