using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("battle/results/[action]")]
    public class BattleResultsController : ControllerBase
    {
        private readonly BattleResultsService _battleResultsService;

        public BattleResultsController(BattleResultsService battleResultsService)
        {
            this._battleResultsService = battleResultsService;
        }


        [HttpGet]
        public async Task<IActionResult> Get(BattleResultsGetResultsRequest request)
        {
            var (content, message) = await this._battleResultsService.GetResults(request);

            var response = new Response<BattleResultsGetResultsResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
