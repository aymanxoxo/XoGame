using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XoGame.Models
{
    public class PlayerScore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid PlayerId { get; set; }

        public Guid MatchId { get; set; }

        public int Score { get; set; }
    }
}