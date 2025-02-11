using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Mvc;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("admin/npcs/[action]")]
    public class AdminNpcsController : ControllerBase
    {
        private readonly AdminNpcsService _adminNpcService;

        public AdminNpcsController(AdminNpcsService adminNpcService)
        {
            this._adminNpcService = adminNpcService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminNpcsCreateRequest request)
        {
            var (content, message) = await this._adminNpcService.Create(request);

            var response = new Response<AdminNpcsCreateResponse>
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Seed(AdminNpcsCreateRequest_seed request)
        {
            var (content, message) = await this._adminNpcService.Seed(request);

            var response = new Response<AdminNpcsCreateResponse>
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> Read(AdminNpcsReadRequest request)
        {
            var (content, message) = await this._adminNpcService.Read(request);

            var response = new Response<List<AdminNpcsReadResponse>>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(AdminNpcsUpdateRequest request)
        {
            var (content, message) = await this._adminNpcService.Update(request);

            var response = new Response<AdminNpcsUpdateResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(AdminNpcsDeleteRequest request)
        {
            var (content, message) = await this._adminNpcService.Delete(request);

            var response = new Response<AdminNpcsDeleteResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }
    }
}