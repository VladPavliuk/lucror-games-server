using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Game
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool Has_levels { get; set; }
        public bool Has_difficulties { get; set; }
    }
}
