using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Seeders
{
    public class GamesInitializare
    {
        private readonly ApplicationDbContext _context;

        List<Game> _dateList = new List<Game>
        {
            new Game()
            {
                Title = "Ping-Pong",
                Url = "https://ping-pong-js-game.herokuapp.com"
            }
        };

        public GamesInitializare(
            ApplicationDbContext context
        )
        {
            _context = context;
        }

        public async Task Seed()
        {
            if (!_context.Game.Any())
            {
                _context.AddRange(_dateList);
                await _context.SaveChangesAsync();
            }
        }
    }
}
