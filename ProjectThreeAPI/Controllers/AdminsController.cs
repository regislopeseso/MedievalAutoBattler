using MedievalAutoBattler.Models.Dtos.Request.Admin;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Models.Dtos.Response.Admin;
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

        [HttpPost]
        public async Task<IActionResult> SeedCards(AdminsSeedCardsRequest request)
        {
            var (content, message) = await this._adminsService.SeedCards(request);

            var response = new Response<AdminsSeedCardsResponse>
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCards(AdminsGetCardsRequest request)//Corrigir a filtragem desse Enpoint
        {
            var (content, message) = await this._adminsService.GetCards(request);

            var response = new Response<List<AdminsGetCardsResponse>>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> EditCard(AdminsEditCardRequest request)//Corrigir a verificação do nome nesse endpoint ele não aceita "" mas aceita "  ", impor no mínimo 3 caracteres
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
        public async Task<IActionResult> CreateNpc(AdminsCreateNpcRequest request)//Corrigir a verificação do nome e descrição nesse endpoint ele não aceita "" mas aceita "  ", impor no mínimo 3 caracteres
        {
            var (content, message) = await this._adminsService.CreateNpc(request);

            var response = new Response<AdminsCreateNpcResponse>
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> SeedNpcs(AdminsSeedNpcsRequest request)
        {
            var (content, message) = await this._adminsService.SeedNpcs(request);

            var response = new Response<AdminsSeedNpcsResponse>
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
        public async Task<IActionResult> EditNpc(AdminsEditNpcRequest request) //Corrigir a verificação do nome e descrição nesse endpoint ele não aceita "" mas aceita "  ", impor no mínimo 3 caracteres e ajustar a filtragem de id's errados para listá-los tal como no edit da carta
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

        [HttpDelete]
        public async Task<IActionResult> DeleteDbData(AdminsDeleteDbDataRequest request)
        {
            var (content, message) = await this._adminsService.DeleteDbData(request);

            var response = new Response<AdminsDeleteDbDataResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
