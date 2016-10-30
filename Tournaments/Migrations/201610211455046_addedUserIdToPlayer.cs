namespace Tournaments.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedUserIdToPlayer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Players", "UserId");
        }
    }
}
