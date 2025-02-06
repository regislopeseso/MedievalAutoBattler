using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Mvc;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("player/saves/[action]")]
    public class PlayerSavesController : ControllerBase
    {
        private readonly PlayerSavesService _playerSavesService;
        public PlayerSavesController(PlayerSavesService playerSavesService)
        {
            this._playerSavesService = playerSavesService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlayerSavesCreateRequest request)
        {
            var (content, message) = await this._playerSavesService.Create(request);

            var response = new Response<PlayerSavesCreateResponse>
            {
                Content = content,
                Message = message,
            };

            return new JsonResult(response);
        }     
    }
}
