using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tournaments.Models
{
    public class PlayerStatsViewModel
    {
        public Player Player { get; set; }

        public int GamesPlayed { get; set; }

        public int Wins { get; set; }

        public int Ties { get; set; }

        public int Losses { get; set; }

        public int ScoresForward { get; set; }

        public int ScoresAgainst { get; set; }

        public int Points { get; set; }
    }
}