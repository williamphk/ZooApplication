namespace Test2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserFavouriteColour : DbMigration
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
