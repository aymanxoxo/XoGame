using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace XoGame.Models
{
    public class Table
    {
        public Table()
        {
            IsLocked = false;
            IsDeleted = false;
            Players = new List<Registered>();
            Matches = new List<Match>();
        }

        public Table(Registered owner) : this()
        {
            Owner = owner;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("OwnerId")]
        public Registered Owner { get; set; }

        public bool IsLocked { get; set; }
        
        public ICollection<Registered> Players { get; set; }

        public List<Match> Matches { get; set; }

        public Guid OwnerId { get; set; }

        public bool IsDeleted { get; set; }

        [NotMapped]
        public int PlayersCount => Players?.Count ?? 0;
    }
}