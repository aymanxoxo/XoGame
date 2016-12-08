using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace XoGame.Models
{
    public class Match
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("TableId")]
        public Table Table { get; set; }

        public Guid TableId { get; set; }

        [ForeignKey("MatchId")]
        public List<PlayerScore> Scores { get; set; }

        public MatchStatus Status { get; set; }
    }
}