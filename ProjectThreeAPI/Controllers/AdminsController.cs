using MedievalAutoBattler.Models.Dtos.Request.Admin;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Models.Dtos.Response.Admin;
using MedievalAutoBattler.Service.Admin;
using Microsoft.AspNetCore.Mvc;

namespace MedievalAutoBattler.Controllers
{
    [ApiController]
    [Route("admins/[action]")]
    public class AdminsController : ControllerBase
    {
        private readonly AdminCardsService _adminCardsService;
        private readonly AdminNpcsService _adminNpcsService;
        private readonly AdminDeleteService _adminDeleteService;


        public AdminsController(AdminCardsService adminCardsService, AdminNpcsService adminNpcsService, AdminDeleteService adminDeleteService)
        {
            this._adminCardsService = adminCardsService;
            this._adminNpcsService = adminNpcsService;
            this._adminDeleteService = adminDeleteService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateCard(AdminsCreateCardRequest request)
        {
            var (content, message) = await this._adminCardsService.Create(request);

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
            var (content, message) = await this._adminCardsService.Seed(request);

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
            var (content, message) = await this._adminCardsService.Get(request);

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
            var (content, message) = await this._adminCardsService.Edit(request);

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
            var (content, message) = await this._adminCardsService.Delete(request);

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
            var (content, message) = await this._adminNpcsService.Create(request);

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
            var (content, message) = await this._adminNpcsService.Seed(request);

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
            var (content, message) = await this._adminNpcsService.Get(request);

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
            var (content, message) = await this._adminNpcsService.Edit(request);

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
            var (content, message) = await this._adminNpcsService.Delete(request);

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
            var (content, message) = await this._adminDeleteService.Delete(request);

            var response = new Response<AdminsDeleteDbDataResponse>()
            {
                Content = content,
                Message = message
            };

            return new JsonResult(response);
        }
    }
}
