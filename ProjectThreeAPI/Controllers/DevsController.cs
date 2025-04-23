using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MedievalAutoBattler.Models.Dtos.Request.Devs;
using MedievalAutoBattler.Models.Dtos.Response.Devs;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("devs/[action]")]
    public class DevsController : ControllerBase
    {
        private readonly DevsService _devsService;

        public DevsController(DevsService devsService)
        {
            this._devsService = devsService;
        }

        [HttpPost]
        public async Task<IActionResult> Seed(DevsSeedRequest request)
        {     
            var (content, message) = await this._devsService.Seed(request);

            var response = new Response<DevsSeedResponse>
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSeed(DevsDeleteSeedRequest? request)
        {
            var (content, message) = await this._devsService.DeleteSeed(request);

            var response = new Response<DevsDeleteSeedResponse>
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

    }
}
