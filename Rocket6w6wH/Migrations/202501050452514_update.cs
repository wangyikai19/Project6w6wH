namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Area = c.String(),
                        CountryName = c.String(),
                        Country = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Configs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Group = c.String(),
                        PVal = c.String(),
                        MVal = c.String(),
                        CreateTime = c.DateTime(),
                        ModifyTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                        Country = c.String(nullable: false, maxLength: 50),
                        Birthday = c.DateTime(),
                        Photo = c.String(),
                        Badge = c.String(),
                        MemberPictureUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Replies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentId = c.Int(nullable: false),
                        ReplyUserId = c.Int(nullable: false),
                        ReplyContent = c.String(),
                        CreateTime = c.DateTime(),
                        ModifyTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.ReplyUserId, cascadeDelete: true)
                .ForeignKey("dbo.StoreComments", t => t.CommentId, cascadeDelete: true)
                .Index(t => t.CommentId)
                .Index(t => t.ReplyUserId);
            
            CreateTable(
                "dbo.ReplyLikes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReplyId = c.Int(nullable: false),
                        LikeUserId = c.Int(nullable: false),
                        CreateTime = c.DateTime(),
                        Member_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.Member_Id)
                .ForeignKey("dbo.Replies", t => t.ReplyId, cascadeDelete: true)
                .Index(t => t.ReplyId)
                .Index(t => t.Member_Id);
            
            CreateTable(
                "dbo.StoreComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreId = c.Int(nullable: false),
                        MemberId = c.Int(nullable: false),
                        Comment = c.String(maxLength: 50),
                        CreateTime = c.DateTime(),
                        ModifyTime = c.DateTime(),
                        Stars = c.Int(nullable: false),
                        Label = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.MemberId)
                .ForeignKey("dbo.Stores", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId)
                .Index(t => t.MemberId);
            
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
            
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreName = c.String(nullable: false, maxLength: 100),
                        AddressCh = c.String(),
                        AddressEn = c.String(),
                        PriceStart = c.Int(),
                        PriceEnd = c.Int(),
                        Phone = c.String(maxLength: 20),
                        ReserveUrl = c.String(),
                        StoreUrl = c.String(),
                        StoreGoogleId = c.String(),
                        BusinessHours = c.String(),
                        XLocation = c.Decimal(precision: 18, scale: 8),
                        YLocation = c.Decimal(precision: 18, scale: 8),
                        StoreTags = c.String(),
                        CreateTime = c.DateTime(),
                        ModifyTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StorePictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreId = c.Int(nullable: false),
                        PictureUrl = c.String(),
                        CreateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stores", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.SearchConditions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Group = c.String(),
                        PVal = c.String(),
                        MVal = c.String(),
                        CreateTime = c.DateTime(),
                        ModifyTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StorePictures", "StoreId", "dbo.Stores");
            DropForeignKey("dbo.StoreComments", "StoreId", "dbo.Stores");
            DropForeignKey("dbo.Replies", "CommentId", "dbo.StoreComments");
            DropForeignKey("dbo.StoreComments", "MemberId", "dbo.Members");
            DropForeignKey("dbo.CommentPictures", "CommentId", "dbo.StoreComments");
            DropForeignKey("dbo.ReplyLikes", "ReplyId", "dbo.Replies");
            DropForeignKey("dbo.ReplyLikes", "Member_Id", "dbo.Members");
            DropForeignKey("dbo.Replies", "ReplyUserId", "dbo.Members");
            DropIndex("dbo.StorePictures", new[] { "StoreId" });
            DropIndex("dbo.CommentPictures", new[] { "CommentId" });
            DropIndex("dbo.StoreComments", new[] { "MemberId" });
            DropIndex("dbo.StoreComments", new[] { "StoreId" });
            DropIndex("dbo.ReplyLikes", new[] { "Member_Id" });
            DropIndex("dbo.ReplyLikes", new[] { "ReplyId" });
            DropIndex("dbo.Replies", new[] { "ReplyUserId" });
            DropIndex("dbo.Replies", new[] { "CommentId" });
            DropTable("dbo.SearchConditions");
            DropTable("dbo.StorePictures");
            DropTable("dbo.Stores");
            DropTable("dbo.CommentPictures");
            DropTable("dbo.StoreComments");
            DropTable("dbo.ReplyLikes");
            DropTable("dbo.Replies");
            DropTable("dbo.Members");
            DropTable("dbo.Configs");
            DropTable("dbo.Cities");
        }
    }
}
