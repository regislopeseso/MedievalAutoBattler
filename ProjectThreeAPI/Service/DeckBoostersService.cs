using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedievalAutoBattler.Service
{
    public class DeckBoostersService
    {
        private readonly ApplicationDbContext _daoDbcontext;
        public DeckBoostersService(ApplicationDbContext daoDbcontext)
        {
            _daoDbcontext = daoDbcontext;
        }

        public async Task<(DeckBoostersCreateResponse, string)> Create(DeckBoostersCreateRequest request)
        {
            var saveDB = await this._daoDbcontext
                                   .Saves
                                   .Include(a => a.Decks)
                                   .ThenInclude(b => b.SaveDeckEntries)
                                   .FirstOrDefaultAsync(a => a.IsDeleted == false && a.Id == request.SaveId);

            var cardsDB = await this._daoDbcontext
                                .Cards
                                .ToListAsync();

            var random = new Random();

            var booster = new List<Card>();
            //pegar 3 cartas aleatórias
            //adicionar ao booster
            //adicionar o booster à coleção do save selecionado

            saveDB.CountBoosters++;
           

            return (null, "");
        }


    }
}
