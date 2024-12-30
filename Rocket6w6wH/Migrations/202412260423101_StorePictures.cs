namespace Rocket6w6wH.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StorePictures : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.StorsPictures", newName: "StorePictures");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.StorePictures", newName: "StorsPictures");
        }
    }
}
