namespace NaseSlovoApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class KorisnikID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "KorisnikID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "KorisnikID");
        }
    }
}
