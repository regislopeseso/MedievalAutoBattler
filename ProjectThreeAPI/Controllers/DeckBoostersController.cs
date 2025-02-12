using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("deck/boosters/[action]")]
    public class DeckBoostersController : ControllerBase
    {
        private readonly DeckBoostersService _deckBoostersService;

        public DeckBoostersController(DeckBoostersService deckBoostersService)
        {
            this._deckBoostersService = deckBoostersService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(DeckBoostersCreateRequest request)
        {
            var (content, message) = await this._deckBoostersService.OpenBooster(request);

            var response = new Response<DeckBoostersCreateResponse>
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
