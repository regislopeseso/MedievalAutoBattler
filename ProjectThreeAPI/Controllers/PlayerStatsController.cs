using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Read(PlayerStatsReadRequest request)
        {
            var (content, message) = await this._playerStatsService.Read(request);

            var response = new Response<PlayerStatsReadResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
