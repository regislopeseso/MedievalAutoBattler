using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectThreeAPI.Models.Dtos.Response;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("match/npcs/[controller]")]
    public class BattleNpcsController : ControllerBase
    {
        private readonly BattleNpcsService _matchNpcsService;
        public BattleNpcsController(BattleNpcsService matchNpcsService)
        {
            _matchNpcsService = matchNpcsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var message = await _matchNpcsService.Create();

            var response = new Response<string>()
            {
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
