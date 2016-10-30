using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tournaments.Models
{
    public class Tournament
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Number of meetings")]
        [Range(1, 30, ErrorMessage = "The number of meetings must be in the range between 1 and 30")]
        public int NumberOfMeetings { get; set; }

        [DisplayName("Tournament name")]
        [Required]
        public string TournamentName { get; set; }

        public string UserId { get; set; }
    }
}