namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SAMACA : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.StoreComments", "StoreId");
            AddForeignKey("dbo.StoreComments", "StoreId", "dbo.Stores", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreComments", "StoreId", "dbo.Stores");
            DropIndex("dbo.StoreComments", new[] { "StoreId" });
        }
    }
}
