using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tournaments.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Player name")]
        public string PlayerName { get; set; }

        public string UserId { get; set; }

    }
}