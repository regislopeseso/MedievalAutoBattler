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

        private readonly PlayerBattlesService _playerBattlesService;
     

        public PlayersController(PlayerSavesService playerSavesService, PlayerCardsService playerCardsService, PlayerDecksService playerDecksService, PlayerStatsService playerStatsService, PlayerBoostersService playerBoostersService, PlayerBattlesService playerBattlesService)
        {
            this._playerSavesService = playerSavesService;
            this._playerCardsService = playerCardsService;
            this._playerDecksService = playerDecksService;
            this._playerStatsService = playerStatsService;
            this._playerBoostersService = playerBoostersService;
            this._playerBattlesService = playerBattlesService;;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSave(PlayersCreateNewSaveRequest request) //Corrigir a verificação do nome e descrição nesse endpoint ele não aceita "" mas aceita "  ", impor no mínimo 3 caracteres
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
        public async Task<IActionResult> GetCards(PlayersGetCardsRequest request)//filtrar para o caso de não informar, nada request == null "Error: no information provided"
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
        public async Task<IActionResult> CreateDeck(PlayersCreateNewDeckRequest request)//Corrigir a verificação do nome e descrição nesse endpoint ele não aceita "" mas aceita "  ", impor no mínimo 3 caracteres e ajustar a filtragem de id's errados para listá-los tal como no edit da carta. Corrigir a filtragem para que SaveCardEntries com iSDeleted == true não possam ser acrescidas ao novo Deck
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
        public async Task<IActionResult> EditDeck(PlayersEditDeckRequest request)//Corrigir a verificação do nome e descrição nesse endpoint ele não aceita "" mas aceita "  ", impor no mínimo 3 caracteres e ajustar a filtragem de id's errados para listá-los tal como no edit da carta. Corrigir a filtragem para que SaveCardEntries com iSDeleted == true não possam ser acrescidas ao novo Deck
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
        public async Task<IActionResult> DeleteDeck(PlayersDeleteDeckRequest request) //Corrigir mensagem de erro para id inválido, atualmente: "Error: Invalid Deck ID, the npc does not exist or is already deleted" e para o caso de nada ser informado
        {
            var (content, message) = await this._playerDecksService.Delete(request);

            var response = new Response<PlayersDeleteDeckResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> OpenBooster(PlayersOpenBoosterRequest request) // Corrigir, booster está acessando cartas com IsDeleted == true
        {
            var (content, message) = await this._playerBoostersService.Open(request);

            var response = new Response<List<PlayersOpenBoosterResponse>>()
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
        public async Task<IActionResult> CreateBattle(PlayersCreateBattleRequest request)//filtrar para o caso de não informar, nada request == null "Error: no information provided"
        {
            var (content, message) = await this._playerBattlesService.Create(request);

            var response = new Response<PlayersCreateBattleResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> PlayBattle(PlayersPlayBattleRequest request) //Considerar se caso uma carta for apagar então os SaveCardEntries também serem apagados, se então qualquer deck contendo elas deve ser setado com isDeleted e caso o saveCardEntry for removido então também o Deck que passará a ter 4 cartas ser setado como isDeleted. Tratar o erro que é obtido caso tentar rodar uma batalha com um deck de tamanho inferior a 5
        {
            var (content, message) = await this._playerBattlesService.Play(request);

            var response = new Response<PlayersPlayBattleResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBattleResults(PlayersGetBattleResultRequest request)
        {
            var (content, message) = await this._playerBattlesService.Get(request);

            var response = new Response<PlayersGetBattleResultResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

     
    }
}
