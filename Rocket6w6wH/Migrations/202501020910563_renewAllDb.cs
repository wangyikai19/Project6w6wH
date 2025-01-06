namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renewAllDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Area = c.String(),
                        CountyName = c.String(),
                        County = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Configs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Group = c.String(),
                        PVal = c.String(),
                        Mavl = c.String(),
                        CreateTime = c.DateTime(),
                        ModifyTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Member",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 50),
                        Name = c.String(),
                        MemberPictureUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StoreComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreId = c.Int(nullable: false),
                        MemberId = c.Int(nullable: false),
                        Comment = c.String(),
                        CreateTime = c.DateTime(),
                        ModifyTime = c.DateTime(),
                        Stars = c.Int(nullable: false),
                        Label = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Member", t => t.MemberId, cascadeDelete: true)
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
                        XLocation = c.Decimal(precision: 18, scale: 0),
                        YLocation = c.Decimal(precision: 18, scale: 0),
                        CreateTime = c.DateTime(),
                        ModifyTime = c.DateTime(),
                        StoreTags = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SearchConditions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Group = c.String(),
                        PVal = c.String(),
                        Mavl = c.String(),
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StoreStars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreId = c.Int(nullable: false),
                        MemberId = c.Int(nullable: false),
                        Stars = c.Int(nullable: false),
                        CreateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreComments", "StoreId", "dbo.Stores");
            DropForeignKey("dbo.StoreComments", "MemberId", "dbo.Member");
            DropForeignKey("dbo.CommentPictures", "CommentId", "dbo.StoreComments");
            DropIndex("dbo.CommentPictures", new[] { "CommentId" });
            DropIndex("dbo.StoreComments", new[] { "MemberId" });
            DropIndex("dbo.StoreComments", new[] { "StoreId" });
            DropTable("dbo.StoreStars");
            DropTable("dbo.StorePictures");
            DropTable("dbo.SearchConditions");
            DropTable("dbo.Stores");
            DropTable("dbo.CommentPictures");
            DropTable("dbo.StoreComments");
            DropTable("dbo.Member");
            DropTable("dbo.Configs");
            DropTable("dbo.Cities");
        }
    }
}
