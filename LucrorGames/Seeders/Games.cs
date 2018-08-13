using LucrorGames.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LucrorGames.Seeders
{
    public class GamesInitializare
    {
        private readonly ApplicationDbContext _context;

        List<Game> _dateList = new List<Game>
        {
            new Game()
            {
                Title = "Ping-Pong",
                Url = "http://localhost:3000"
            },
            new Game() {
                Title = "Snake",
                Url = "http://localhost:3001"
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
