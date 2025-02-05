using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Entities;
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
            var message = await this._playerDecksService.Create(newDeck);

            var response = new Response<string>
            {
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(PlayerDecksUpdateRequest deck)
        {
            var message = await this._playerDecksService.Update(deck);

            var response = new Response<string>
            {
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int deckId)
        {
            var message = await this._playerDecksService.Delete(deckId);

            var response = new Response<string>()
            {
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
