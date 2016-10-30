namespace Tournaments.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addetUserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tournaments", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tournaments", "UserId");
        }
    }
}
