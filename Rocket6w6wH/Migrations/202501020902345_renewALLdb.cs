namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renewALLdb : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StoreComments", "Stores_Id", "dbo.Stores");
            DropIndex("dbo.StoreComments", new[] { "Stores_Id" });
            DropColumn("dbo.StoreComments", "StoreId");
            RenameColumn(table: "dbo.StoreComments", name: "Stores_Id", newName: "StoreId");
            AlterColumn("dbo.StoreComments", "StoreId", c => c.Int(nullable: false));
            CreateIndex("dbo.StoreComments", "StoreId");
            AddForeignKey("dbo.StoreComments", "StoreId", "dbo.Stores", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreComments", "StoreId", "dbo.Stores");
            DropIndex("dbo.StoreComments", new[] { "StoreId" });
            AlterColumn("dbo.StoreComments", "StoreId", c => c.Int());
            RenameColumn(table: "dbo.StoreComments", name: "StoreId", newName: "Stores_Id");
            AddColumn("dbo.StoreComments", "StoreId", c => c.Int(nullable: false));
            CreateIndex("dbo.StoreComments", "Stores_Id");
            AddForeignKey("dbo.StoreComments", "Stores_Id", "dbo.Stores", "Id");
        }
    }
}
