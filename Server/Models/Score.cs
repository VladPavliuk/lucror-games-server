using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Score
    {
        public long Id { get; set; }
        public ApplicationUser User { get; set; }
        public Game Game { get; set; }
        public decimal Value { get; set; }
    }
}
