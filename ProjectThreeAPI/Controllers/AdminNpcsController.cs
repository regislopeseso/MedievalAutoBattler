using Microsoft.AspNetCore.Mvc;
using ProjectThreeAPI.Models.Dtos.Request;
using ProjectThreeAPI.Models.Dtos.Response;
using ProjectThreeAPI.Service;

namespace ProjectThreeAPI.Controllers
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
        public async Task<IActionResult> Create(AdminNpcsCreateRequest npc)
        {
            var message = await this._adminNpcService.Create(npc);

            var response = new Response<string>
            {
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> Read()
        {
            var (npcs, message) = await this._adminNpcService.Read();

            var response = new Response<List<AdminNpcsReadResponse>>()
            {
                Content = npcs,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(AdminNpcsUpdateRequest npc)
        {
            var message = await this._adminNpcService.Update(npc);

            var response = new Response<string>()
            {
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int npcId)
        {
            var message = await this._adminNpcService.Delete(npcId);

            var response = new Response<string>()
            {
                Message = message
            };

            return new JsonResult(response);
        }
    }
}