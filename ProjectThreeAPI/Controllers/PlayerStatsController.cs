using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Mvc;
using ProjectThreeAPI.Models.Dtos.Response;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("player/stats/[action]")]
    public class PlayerStatsController : ControllerBase
    {
        private readonly PlayerStatsService _playerStatsService;
        public PlayerStatsController(PlayerStatsService playerStatsService)
        {
            this._playerStatsService = playerStatsService;
        }

        [HttpGet]
        public async Task<IActionResult> Read(int saveId)
        {
            var (result, message) = await this._playerStatsService.Read(saveId);

            var response = new Response<PlayerStatsReadResponse>()
            {
                Content = result,
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
