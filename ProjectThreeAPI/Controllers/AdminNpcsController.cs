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
        private readonly AdminNpcService _adminNpcService;

        public AdminNpcsController(AdminNpcService adminNpcService)
        {
            this._adminNpcService = adminNpcService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminNpcCreateRequest npc)
        {
            var message = await _adminNpcService.Create(npc);

            var response = new Response<string>
            {
                Message = message
            };

            return new JsonResult(response);
        }

        //[HttpGet]
        //public async Task<IActionResult> Read()
        //{
        //    var npcs = await _adminNpcService.Read();

        //    var response = new Response<List<NpcReadAdminResponse>>()
        //    {
        //        Content = npcs,
        //    };

        //    return new JsonResult(response);
        //}

        //[HttpPut]
        //public async Task<IActionResult> Update(NpcUpdateAdminRequest npc)
        //{
        //    var message = await _adminNpcService.Update(npc);

        //    var response = new Response<string>()
        //    {
        //        Message = message
        //    };

        //    return new JsonResult(response);
        //}

        //[HttpDelete]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var message = await _adminNpcService.Delete(id);

        //    var response = new Response<string>()
        //    {
        //        Message = message
        //    };

        //    return new JsonResult(response);
        //}
    }
}