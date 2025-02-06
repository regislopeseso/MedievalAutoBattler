using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Entities;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectThreeAPI.Models.Dtos.Response;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("battle/players/[action]")]
    public class BattlePlayersController : ControllerBase
    {
        private readonly BattlePlayersService _battlePlayersService;
        public BattlePlayersController(BattlePlayersService battlePlayersService)
        {
            _battlePlayersService = battlePlayersService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(BattlePlayersCreateRequest newBattle)
        {
            var message = await _battlePlayersService.Create(newBattle);

            var response = new Response<string>()
            {
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
