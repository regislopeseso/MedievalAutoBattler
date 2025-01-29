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
        public async Task<IActionResult> Create(CardCreateAdminRequest card)
        {
            var (result, message) = await _adminCardService.Create(card);

            var response = new Response<CardCreateAdminResponse>()
            {
                Content = result,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> Read()
        {
            var cards = await _adminCardService.Read();

            var response = new Response<List<CardReadAdminResponse>>()
            {
                Content = cards,
            };

            return new JsonResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id)
        {
            var updatedCard = await _adminCardService.Update(id);

            var response = new Response<CardUpdateAdminResponse>()
            {
                Content = updatedCard,
            };

            return new JsonResult(response);
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            return null;
        }

    }
}
