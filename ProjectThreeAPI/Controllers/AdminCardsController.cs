using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Mvc;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("admin/cards/[action]")]
    public class AdminCardsController : ControllerBase
    {
        private readonly AdminCardsService _adminCardService;

        public AdminCardsController(AdminCardsService adminCardService)
        {
            this._adminCardService = adminCardService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminCardsCreateRequest request)
        {
            var (content, message) = await this._adminCardService.Create(request);

            var response = new Response<AdminCardsCreateResponse>
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> Read([FromQuery] AdminCardsReadRequest request)
        {
            var (content, message) = await this._adminCardService.Read();

            var response = new Response<List<AdminCardsReadResponse>>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(AdminCardsUpdateRequest request)
        {
            var (content, message) = await this._adminCardService.Update(request);

            var response = new Response<AdminCardsUpdateResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(AdminCardsDeleteRequest request)
        {
            var (content, message) = await this._adminCardService.Delete(request);

            var response = new Response<AdminCardsDeleteResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
