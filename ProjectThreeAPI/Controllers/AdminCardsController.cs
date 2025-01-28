using Microsoft.AspNetCore.Mvc;
using ProjectThreeAPI.Models.Dtos.Request;
using ProjectThreeAPI.Models.Dtos.Response;
using ProjectThreeAPI.Service;

namespace ProjectThreeAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
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
        public IActionResult Read()
        {
            return null;
        }

        [HttpPut]
        public IActionResult Update()
        {
            return null;
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            return null;
        }

    }
}
