using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tournaments.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        public DateTime? GameDate { get; set; }

        public DateTime GameCreated{ get; set; }

        public int AwayPlayerId { get; set; }

        [Range(0, 100, ErrorMessage = "The score must be in the range between 0 and 100")]
        public int? AwayPlayerScore { get; set; }

        public int HomePlayerId { get; set; }

        [Range(0, 100, ErrorMessage = "The score must be in the range between 0 and 100")]
        public int? HomePlayerScore { get; set; }

        public int TournamentId { get; set; }

        public int? Round { get; set; }
    }
}