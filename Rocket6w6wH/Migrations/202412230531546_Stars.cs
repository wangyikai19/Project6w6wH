namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stars : DbMigration
    {
        public override void Up()
        {
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
            DropTable("dbo.StoreStars");
        }
    }
}
