using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Mvc;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("player/decks/[action]")]
    public class PlayerDecksController : ControllerBase
    {
        private readonly PlayerDecksService _playerDecksService;

        public PlayerDecksController(PlayerDecksService playerDecksService)
        {
            this._playerDecksService = playerDecksService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlayerDecksCreateRequest request)
        {
            var (content, message) = await this._playerDecksService.Create(request);

            var response = new Response<PlayerDecksCreateResponse>
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(PlayerDecksUpdateRequest request)
        {
            var (content, message) = await this._playerDecksService.Update(request);

            var response = new Response<PlayerDecksUpdateResponse>
            {
                Content= content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(PlayerDecksDeleteRequest request)
        {
            var (content, message) = await this._playerDecksService.Delete(request);

            var response = new Response<PlayerDecksDeleteResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
