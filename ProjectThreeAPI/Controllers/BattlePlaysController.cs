using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("battle/plays/[action]")]
    public class BattlePlaysController : ControllerBase
    {
        private readonly BattlePlaysService _battlePlayService;

        public BattlePlaysController(BattlePlaysService battlePlayersService)
        {
            this._battlePlayService = battlePlayersService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(BattlePlaysRunRequest request)
        {
            var (content, message) = await this._battlePlayService.Run(request);

            var response = new Response<BattlePlaysRunResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get(BattlePlaysGetRequest request)
        {
            var (content, message) = await this._battlePlayService.GetResults(request);

            var response = new Response<BattlePlaysGetResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }


    }
}
