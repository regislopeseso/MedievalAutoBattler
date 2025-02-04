using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Mvc;
using ProjectThreeAPI.Models.Dtos.Response;

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
        public async Task<IActionResult> Read(int saveId)
        {
            var (result, message) = await this._playerCardsService.Read(saveId);

            var response = new Response<List<PlayerCardsReadResponse>>()
            {
                Content = result,
                Message = message               
            };

            return new JsonResult(response);
        }
    }
}
