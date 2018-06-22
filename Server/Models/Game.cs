using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LucrorGames.Models
{
    public class Game
    {
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Url { get; set; }
        public bool HasLevels { get; set; } = false;
        public bool HasDifficulties { get; set; } = false;
    }
}
