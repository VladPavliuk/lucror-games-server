using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucrorGames.Models
{
    public class Score
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public ApplicationUser User { get; set; }
        public Game Game { get; set; }
        public decimal Value { get; set; }
    }
}
