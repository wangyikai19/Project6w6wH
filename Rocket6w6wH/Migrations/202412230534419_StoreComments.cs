namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreComments : DbMigration
    {
        public override void Up()
        {
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
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.StoreComments");
        }
    }
}
