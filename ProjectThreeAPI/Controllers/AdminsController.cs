using MedievalAutoBattler.Models.Dtos.Request.Admins;
using MedievalAutoBattler.Models.Dtos.Request.Devs;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Models.Dtos.Response.Admins;
using MedievalAutoBattler.Models.Dtos.Response.Devs;
using MedievalAutoBattler.Service;
using Microsoft.AspNetCore.Mvc;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("admins/[action]")]
    public class AdminsController : ControllerBase
    {
        private readonly AdminsService _adminsService;

        public AdminsController(AdminsService adminsService)
        {
            this._adminsService = adminsService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCard(AdminsCreateCardRequest request)
        {
            var (content, message) = await this._adminsService.CreateCard(request);

            var response = new Response<AdminsCreateCardResponse>
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }        

        [HttpGet]
        public async Task<IActionResult> FilterCards(AdminsFilterCardsRequest request)
        {
            var (content, message) = await this._adminsService.FilterCards(request);

            var response = new Response<List<AdminsFilterCardsResponse>>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCards(AdminsGetAllCardsRequest request)
        {
            var (content, message) = await this._adminsService.GetAllCards(request);

            var response = new Response<List<AdminsGetAllCardsResponse>>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> EditCard(AdminsEditCardRequest request)
        {
            var (content, message) = await this._adminsService.EditCards(request);

            var response = new Response<AdminsEditCardResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCard(AdminsDeleteCardRequest request)
        {
            var (content, message) = await this._adminsService.DeleteCards(request);

            var response = new Response<AdminsDeleteCardResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNpc(AdminsCreateNpcRequest request)
        {
            var (content, message) = await this._adminsService.CreateNpc(request);

            var response = new Response<AdminsCreateNpcResponse>
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }
                  
        [HttpGet]
        public async Task<IActionResult> GetNpcs(AdminsGetNpcsRequest request) //Corrigir a filtragem desse Enpoint
        {
            var (content, message) = await this._adminsService.GetNpcs(request);

            var response = new Response<List<AdminsGetNpcsResponse>>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> EditNpc(AdminsEditNpcRequest request) //Ajustar a filtragem de id's errados para listá-los tal como no edit da carta
        {
            var (content, message) = await this._adminsService.EditNpc(request);

            var response = new Response<AdminsEditNpcResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteNpc(AdminsDeleteNpcRequest request) //filtrar para o caso de não informar, nada request == null "Error: no information provided"
        {
            var (content, message) = await this._adminsService.DeleteNpc(request);

            var response = new Response<AdminsDeleteNpcResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

    }
}
