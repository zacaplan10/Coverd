using BlackjackGameEngine.GameEngine;
using Databases;
using Microsoft.AspNetCore.Mvc;
using BlackjackState = CoverdWebApi.Models.GameModels.BlackjackState;

namespace CoverdWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlackjackController : ControllerBase
    {
        private IDatabase Database { get; }
        private static Dictionary<int, BlackjackGame> ActiveGames = new();
        
        public BlackjackController(IDatabase database)
        {
            this.Database = database;
        }
        
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("BlackjackController is alive");
        }

        [HttpPost("start")]
        public ActionResult<BlackjackState> StartGame([FromQuery] int userId, [FromQuery] int bet)
        {
            if (!ActiveGames.TryGetValue(userId, out var game))
            {
                game = new BlackjackGame(Database, 4, userId);
                ActiveGames[userId] = game;
            }

            if (!game.StartNewRound(bet))
            {
                return BadRequest("Insufficient balance, please add more funds.");
            }

            return Ok(game.GetState());
        }

        [HttpPost("hit")]
        public ActionResult<BlackjackState> Hit([FromQuery] int userId)
        {
            if (!ActiveGames.TryGetValue(userId, out var game)) return NotFound();

            game.PlayerHit();
            return Ok(game.GetState());
        }

        [HttpPost("stand")]
        public ActionResult<BlackjackState> Stand([FromQuery] int userId)
        {
            if (!ActiveGames.TryGetValue(userId, out var game)) return NotFound();

            game.PlayerStand();
            return Ok(game.GetState());
        }

        [HttpGet("state")]
        public ActionResult<BlackjackState> GetState([FromQuery] int userId)
        {
            if (!ActiveGames.TryGetValue(userId, out var game)) return NotFound();

            return Ok(game.GetState());
        }
    }

}
