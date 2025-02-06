using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Mvc;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("battle/saves/[action]")]
    public class BattleSavesController : ControllerBase
    {
        private readonly BattleSavesService _battleSavesService;
        public BattleSavesController(BattleSavesService battlePlayersService)
        {
            this._battleSavesService = battlePlayersService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(BattleSavesCreateRequest request)
        {
            var (content, message) = await this._battleSavesService.Create(request);

            var response = new Response<BattleSavesCreateResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
