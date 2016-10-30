using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tournaments.Models
{
    public class ScheduleViewModel
    {
        public List<Game> Games { get; set; }

        public List<int> NumberOfMeetings { get; set; }

        public List<Player> Players { get; set; }

        public bool TournamentAdministrator { get; set; }
    }
}