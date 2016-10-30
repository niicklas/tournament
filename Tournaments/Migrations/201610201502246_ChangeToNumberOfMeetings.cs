namespace Tournaments.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeToNumberOfMeetings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tournaments", "NumberOfMeetings", c => c.Int(nullable: false));
            DropColumn("dbo.Tournaments", "Rounds");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tournaments", "Rounds", c => c.Int(nullable: false));
            DropColumn("dbo.Tournaments", "NumberOfMeetings");
        }
    }
}
