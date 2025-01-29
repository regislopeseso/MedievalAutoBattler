using Microsoft.AspNetCore.Mvc;
using ProjectThreeAPI.Models.Dtos.Request;
using ProjectThreeAPI.Models.Dtos.Response;
using ProjectThreeAPI.Service;

namespace ProjectThreeAPI.Controllers
{
    [ApiController]
    [Route("admin/cardnpc/[action]")]
    public class AdminCardNpcController : Controller
    {

        private readonly AdminCardNpcService _adminCardNpcService;
        public AdminCardNpcController(AdminCardNpcService adminCardNpcService)
        {

            this._adminCardNpcService = adminCardNpcService;
        }

        public async Task<IActionResult> Create(CardNpcCreateRequest cardnpc)
        {
            var (result, message) = await _adminCardNpcService.Create(cardnpc);

            var response = new Response<CardNpcCreateResponse>()
            {
                Content = result,
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
