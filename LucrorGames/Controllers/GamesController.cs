using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LucrorGames.Models;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace LucrorGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GamesController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager
        )
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Games
        [HttpGet]
        public IEnumerable<Game> GetGame()
        {
            return _context.Game;
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGame([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var game = await _context.Game.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        // GET: api/Games/5
        [HttpPost("{id}/open-session")]
        [Authorize]
        public async Task<IActionResult> OpenSession([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var game = await _context.Game.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(50);
            for (int i = 0; i < 50; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            var name = User.Identity.Name;
            ApplicationUser user = await _userManager.FindByNameAsync(name);
            _context.Score.Add(new Score()
            {
                Token = result.ToString(),
                User = user,
                Game = game,
                Value = 0
            });
            await _context.SaveChangesAsync();

            return new JsonResult(new { game = game, token = result.ToString() });
        }

        [HttpPost("close-session/{token}/{score}")]
        [Authorize]
        public IActionResult CloseSession([FromRoute] string token, [FromRoute] long score)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var scoreRecord = _context.Score.Where(item => item.Token == token).FirstOrDefault();

            if (scoreRecord == null)
            {
                return NotFound();
            }

            scoreRecord.Value = score;

            _context.SaveChanges();

            return Ok();
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame([FromRoute] long id, [FromBody] Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != game.Id)
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Games
        [HttpPost]
        public async Task<IActionResult> PostGame([FromBody] Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Game.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGame", new { id = game.Id }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Game.Remove(game);
            await _context.SaveChangesAsync();

            return Ok(game);
        }

        private bool GameExists(long id)
        {
            return _context.Game.Any(e => e.Id == id);
        }
    }
}