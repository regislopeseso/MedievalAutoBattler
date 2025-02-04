using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Mvc;
using ProjectThreeAPI.Models.Dtos.Response;

namespace MedievalAutoBattler.Models.Dtos.Request
{
    [ApiController]
    [Route("player/decks/[action]")]
    public class PlayerDecksController : ControllerBase
    {
        private readonly PlayerDecksService _playerDecksService;

        public PlayerDecksController(PlayerDecksService playerDecksService)
        {
            _playerDecksService = playerDecksService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(List<int> cardIds)
        {
            var message = await this._playerDecksService.Create(cardIds);

            var response = new Response<string>
            {
                Message = message
            };

            return new JsonResult(response);
        }

    }
}
