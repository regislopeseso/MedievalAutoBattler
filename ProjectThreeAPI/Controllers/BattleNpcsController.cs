using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using ProjectThreeAPI.Models.Dtos.Response;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("battle/npcs/[action]")]
    public class BattleNpcsController : ControllerBase
    {
        private readonly BattleNpcsService _battleNpcsService;
        public BattleNpcsController(BattleNpcsService battleNpcsService)
        {
            _battleNpcsService = battleNpcsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(BattleNpcsCreateRequest request)
        {
            var message = await _battleNpcsService.Create(request);

            var response = new Response<string>()
            {
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> Read(int battleId)
        {
            var (npcName, message) = await _battleNpcsService.Read(battleId);

            var response = new Response<BattleNpcsReadResponse>()
            {
                Content = npcName,
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
