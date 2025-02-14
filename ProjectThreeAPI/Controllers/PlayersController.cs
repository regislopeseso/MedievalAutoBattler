using MedievalAutoBattler.Models.Dtos.Request.Players;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Models.Dtos.Response.Players;
using MedievalAutoBattler.Service.Players;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("players/[action]")]
    public class PlayersController : ControllerBase
    {
        private readonly PlayersSavesService _playersSavesService;
        private readonly PlayersCardsService _playersCardsService;
        private readonly PlayersDecksService _playersDecksService;
        private readonly PlayersStatsService _playersStatsService;
        private readonly PlayersBoostersService _playersBoostersService;

        public PlayersController(PlayersSavesService playersSavesService, PlayersCardsService playersCardsService, PlayersDecksService playersDecksService, PlayersStatsService playersStatsService, PlayersBoostersService playersBoostersService)
        {
            this._playersSavesService = playersSavesService;
            this._playersCardsService = playersCardsService;
            this._playersDecksService = playersDecksService;
            this._playersStatsService = playersStatsService;
            this._playersBoostersService = playersBoostersService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSave(PlayersCreateNewSaveRequest request)
        {
            var (content, message) = await this._playersSavesService.Create(request);

            var response = new Response<PlayersCreateNewSaveResponse>
            {
                Content = content,
                Message = message,
            };

            return new JsonResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCards(PlayersGetCardsRequest request)
        {
            var (content, message) = await this._playersCardsService.Get(request);

            var response = new Response<List<PlayersGetCardsResponse>>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeck(PlayersCreateNewDeckRequest request)
        {
            var (content, message) = await this._playersDecksService.Create(request);

            var response = new Response<PlayersCreateNewDeckResponse>
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> EditDeck(PlayersEditDeckRequest request)
        {
            var (content, message) = await this._playersDecksService.Edit(request);

            var response = new Response<PlayersEditDeckResponse>
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDeck(PlayersDeleteDeckRequest request) //PlayersDeleteDeck
        {
            var (content, message) = await this._playersDecksService.Delete(request);

            var response = new Response<PlayersDeleteDeckResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetStats(PlayersGetStatsRequest request) 
        {
            var (content, message) = await this._playersStatsService.Get(request);

            var response = new Response<PlayersGetStatsResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> OpenBooster(PlayersOpenBoosterRequest request)
        {
            var (content, message) = await this._playersBoostersService.Open(request);

            var response = new Response<List<PlayersOpenBoosterResponse>>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
