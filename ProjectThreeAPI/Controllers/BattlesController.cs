using MedievalAutoBattler.Models.Dtos.Request.Battles;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Models.Dtos.Response.Battles;
using MedievalAutoBattler.Service.Battles;
using Microsoft.AspNetCore.Mvc;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("battles/[action]")]
    public class BattlesController : ControllerBase
    {
        private readonly BattlesNewBattlesService _battlesNewBattlesService;
        private readonly BattlesPlaysService _battlesPlaysService;
        private readonly BattlesResultsService _battlesResultsService;
        public BattlesController(BattlesNewBattlesService battlesNewBattlesService, BattlesPlaysService battlePlaysService, BattlesResultsService battleResultsService)
        {
            this._battlesNewBattlesService = battlesNewBattlesService;
            this._battlesPlaysService = battlePlaysService;
            this._battlesResultsService = battleResultsService;
        }     

        [HttpPost]
        public async Task<IActionResult> CreateBattle(BattlesNewBattleCreateRequest request)
        {
            var (content, message) = await this._battlesNewBattlesService.Create(request);

            var response = new Response<BattlesNewBattleCreateResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> PlayBattle(BattlesPlayBattleExecuteRequest request)
        {
            var (content, message) = await this._battlesPlaysService.Play(request);

            var response = new Response<BattlesPlayBattleExecuteResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get(BattlesGetResultsRequest request)
        {
            var (content, message) = await this._battlesResultsService.GetResults(request);

            var response = new Response<BattlesGetResultsResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }


    }
}
