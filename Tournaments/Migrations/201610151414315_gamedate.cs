namespace Tournaments.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gamedate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "GameCreated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Games", "GameDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Games", "GameDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Games", "GameCreated");
        }
    }
}
