using Microsoft.AspNetCore.Mvc;
using ProjectThreeAPI.Models.Dtos.Request;
using ProjectThreeAPI.Models.Dtos.Response;
using ProjectThreeAPI.Service;

namespace ProjectThreeAPI.Controllers
{
    [ApiController]
    [Route("admin/cards/[action]")]
    public class AdminCardsController : ControllerBase
    {
        private readonly AdminCardService _adminCardService;

        public AdminCardsController(AdminCardService adminCardService)
        {
            this._adminCardService = adminCardService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminCardCreateRequest card)
        {
            var message = await _adminCardService.Create(card);

            var response = new Response<string>
            {
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> Read()
        {
            var (response, message) = await _adminCardService.Read();

            var result = new Response<List<AdminCardReadResponse>>()
            {
                Content = response,
                Message = message
            };

            return new JsonResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(AdminCardUpdateRequest card)
        {
            var message = await _adminCardService.Update(card);

            var response = new Response<string>()
            {
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _adminCardService.Delete(id);

            var response = new Response<string>()
            {
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
