using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tournaments.Models
{
    public class PlayerCreateViewModel
    {
        public List<Player> ExistingPlayers { get; set; }

        public List<string> NewPlayerList { get; set; }
    }
}