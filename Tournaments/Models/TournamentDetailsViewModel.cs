using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Tournaments.Models
{
    public class TournamentDetailsViewModel
    {
        public Tournament Tournament { get; set; }

        public ScheduleViewModel Schedule { get; set; }

        public List<PlayerStatsViewModel> Standings { get; set; }

        [DisplayName("Enter an email address to send a link for this thournament")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string EmailAddress { get; set; }
    }
}