using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tournaments.Models
{
    public class TournamentCreateViewModel
    {
        public Tournament Tournament { get; set; }

        [DisplayName("Amount of teams")]
        [Range(2, 30, ErrorMessage = "The numbr of teams must be in the range between 2 and 30")]
        public int AmountOfTeams { get; set; } = 2;
    }
}