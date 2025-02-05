using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Mvc;
using ProjectThreeAPI.Models.Dtos.Response;

namespace MedievalAutoBattler.Controllers
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
        public async Task<IActionResult> Create(PlayerDecksCreateRequest newDeck)
        {
            var message = await _playerDecksService.Create(newDeck);

            var response = new Response<string>
            {
                Message = message
            };

            return new JsonResult(response);
        }

    }
}
