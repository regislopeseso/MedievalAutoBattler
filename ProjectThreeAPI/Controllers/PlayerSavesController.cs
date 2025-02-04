using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Mvc;
using ProjectThreeAPI.Models.Dtos.Response;

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
        public async Task<IActionResult> Create(PlayerSavesCreateRequest newSave)
        {
            var message = await this._playerSavesService.Create(newSave);

            var response = new Response<string>
            {
                Message = message,
            };

            return new JsonResult(response);
        }     
    }
}
