namespace ZooApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserFlavourColour : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FavouriteColour", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "FavouriteColour");
        }
    }
}
