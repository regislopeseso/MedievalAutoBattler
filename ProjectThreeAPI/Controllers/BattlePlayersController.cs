using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Mvc;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("battle/players/[action]")]
    public class BattlePlayersController : ControllerBase
    {
        private readonly BattlePlayersService _battlePlayersService;
        public BattlePlayersController(BattlePlayersService battlePlayersService)
        {
            this._battlePlayersService = battlePlayersService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(BattlePlayersCreateRequest request)
        {
            var (content, message) = await this._battlePlayersService.Create(request);

            var response = new Response<BattlePlayersCreateResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
