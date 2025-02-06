using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Mvc;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("battle/npcs/[action]")]
    public class BattleNpcsController : ControllerBase
    {
        private readonly BattleNpcsService _battleNpcsService;
        public BattleNpcsController(BattleNpcsService battleNpcsService)
        {
            this._battleNpcsService = battleNpcsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(BattleNpcsCreateRequest request)
        {
            var (content, message) = await this._battleNpcsService.Create(request);

            var response = new Response<BattleNpcsCreateResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> Read(BattleNpcsReadRequest request)
        {
            var (content, message) = await this._battleNpcsService.Read(request);

            var response = new Response<BattleNpcsReadResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
