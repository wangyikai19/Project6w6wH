namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SearchCondition : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SearchConditions");
        }
    }
}
