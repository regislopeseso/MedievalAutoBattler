using Microsoft.AspNetCore.Mvc;

namespace ProjectThreeAPI.Controllers
{
    public class AdminsController : Controller
    {

        [HttpPost]
        public IActionResult CreateCard()
        {
            return View();
        }

        [HttpPut]
        public IActionResult EditCard()
        {
            return View();
        }

        [HttpDelete]
        public IActionResult DeleteCard()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateNpc()
        {
            return View();
        }

        [HttpPut]
        public IActionResult EditNpc()
        {
            return View();
        }

        [HttpDelete]
        public IActionResult DeleteNpc()
        {
            return View();
        }
    }
}
