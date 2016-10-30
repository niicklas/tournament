namespace Tournaments.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class round : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "round", c => c.Int());
            AlterColumn("dbo.Games", "AwayPlayerScore", c => c.Int());
            AlterColumn("dbo.Games", "HomePlayerScore", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Games", "HomePlayerScore", c => c.Int(nullable: false));
            AlterColumn("dbo.Games", "AwayPlayerScore", c => c.Int(nullable: false));
            DropColumn("dbo.Games", "round");
        }
    }
}
