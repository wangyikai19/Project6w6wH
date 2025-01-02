namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commit : DbMigration
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
            
            AddColumn("dbo.Member", "MemberPictureUrl", c => c.String());
            AddColumn("dbo.StoreComments", "Label", c => c.String());
            CreateIndex("dbo.StoreComments", "StoreId");
            AddForeignKey("dbo.StoreComments", "StoreId", "dbo.Stores", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreComments", "StoreId", "dbo.Stores");
            DropForeignKey("dbo.CommentPictures", "CommentId", "dbo.StoreComments");
            DropIndex("dbo.CommentPictures", new[] { "CommentId" });
            DropIndex("dbo.StoreComments", new[] { "StoreId" });
            DropColumn("dbo.StoreComments", "Label");
            DropColumn("dbo.Member", "MemberPictureUrl");
            DropTable("dbo.CommentPictures");
        }
    }
}
