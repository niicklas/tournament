namespace Tournaments.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameDate = c.DateTime(nullable: false),
                        AwayPlayerId = c.Int(nullable: false),
                        AwayPlayerScore = c.Int(nullable: false),
                        HomePlayerId = c.Int(nullable: false),
                        HomePlayerScore = c.Int(nullable: false),
                        TournamentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlayerName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tournaments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rounds = c.Int(nullable: false),
                        TournamentName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tournaments");
            DropTable("dbo.Players");
            DropTable("dbo.Games");
        }
    }
}
