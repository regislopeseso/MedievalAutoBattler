using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Mvc;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("player/cards/[action]")]
    public class PlayerCardsController : ControllerBase
    {
        private readonly PlayerCardsService _playerCardsService;

        public PlayerCardsController(PlayerCardsService playerCardsService)
        {
            this._playerCardsService = playerCardsService;
        }

        [HttpGet]
        public async Task<IActionResult> Read(PlayerCardsReadRequest request)
        {
            var (content, message) = await this._playerCardsService.Read(request);

            var response = new Response<List<PlayerCardsReadResponse>>()
            {
                Content = content,
                Message = message               
            };

            return new JsonResult(response);
        }
    }
}
