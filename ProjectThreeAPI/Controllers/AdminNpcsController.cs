using Microsoft.AspNetCore.Mvc;
using ProjectThreeAPI.Models.Dtos.Request;
using ProjectThreeAPI.Service;

namespace ProjectThreeAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AdminNpcsController : ControllerBase
    {
        private readonly AdminCardService _adminService;

        public AdminNpcsController()
        {
            
        }

        [HttpPost]
        public IActionResult Create([FromBody]CardCreateAdminRequest card)
        {
            return null;
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
