using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Tournaments.Models;

namespace Tournaments.Service.Context
{
    public class TournamentContext : DbContext
    {
        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<TournamentContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}