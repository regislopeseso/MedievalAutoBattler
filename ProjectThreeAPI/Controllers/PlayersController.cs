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
        private readonly PlayerSavesService _playerSavesService;
        private readonly PlayerCardsService _playerCardsService;
        private readonly PlayerDecksService _playerDecksService;
        private readonly PlayerStatsService _playerStatsService;
        private readonly PlayerBoostersService _playerBoostersService;

        public PlayersController(PlayerSavesService playerSavesService, PlayerCardsService playerCardsService, PlayerDecksService playerDecksService, PlayerStatsService playerStatsService, PlayerBoostersService playerBoostersService)
        {
            this._playerSavesService = playerSavesService;
            this._playerCardsService = playerCardsService;
            this._playerDecksService = playerDecksService;
            this._playerStatsService = playerStatsService;
            this._playerBoostersService = playerBoostersService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSave(PlayersCreateNewSaveRequest request)
        {
            var (content, message) = await this._playerSavesService.Create(request);

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
            var (content, message) = await this._playerCardsService.Get(request);

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
            var (content, message) = await this._playerDecksService.Create(request);

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
            var (content, message) = await this._playerDecksService.Edit(request);

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
            var (content, message) = await this._playerDecksService.Delete(request);

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
            var (content, message) = await this._playerStatsService.Get(request);

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
            var (content, message) = await this._playerBoostersService.Open(request);

            var response = new Response<List<PlayersOpenBoosterResponse>>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
