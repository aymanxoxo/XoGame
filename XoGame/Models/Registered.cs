using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XoGame.Models
{
    public class Registered : Player
    {
        [Required]
        public string Password { get; set; }

    }
}