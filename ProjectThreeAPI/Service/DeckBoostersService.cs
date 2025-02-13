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

        public async Task<(List<DeckBoostersCreateResponse>?, string)> OpenBooster(DeckBoostersCreateRequest request)
        {
            if(request.SaveId <= 0)
            {
                return (null, "Error: invalid SaveId");
            }
            var newSaveDeckEntries = new List<SaveDeckEntry>();
            var random = new Random();

            var saveDB = await this._daoDbcontext
                                   .Saves
                                   .Include(a => a.Decks)
                                   .ThenInclude(a => a.SaveDeckEntries)
                                   .ThenInclude(a => a.Card)
                                   .FirstOrDefaultAsync(a => a.IsDeleted == false && a.Id == request.SaveId);

            if (saveDB == null)
            {
                return (null, "Error: save not found");
            }
            if(saveDB.Gold < 5)
            {
                return (null, "Error: not enough gold pieces");
            }

            var allCardsIdDB = await this._daoDbcontext
                                .Cards
                                .Select(a => a.Id)
                                .ToListAsync();

            if(allCardsIdDB == null || allCardsIdDB.Count == 0)
            {
                return (null, "Error: no cards available");
            }
            
            var booster = allCardsIdDB.OrderBy(a => random.Next()).Take(3).ToList();

            if (booster == null || booster.Count == 0)
            {
                return (null, "Error: no booster available");
            }

            var content = new List<DeckBoostersCreateResponse>();

            foreach (var cardId in booster)
            {
                content.Add(new DeckBoostersCreateResponse
                {
                    CardId = cardId
                });
            }




            saveDB.Decks.Add(new Deck
            {
                Name = "Booster " + saveDB.CountBoosters + 1,
                SaveDeckEntries = newSaveDeckEntries
            });
            saveDB.CountBoosters++;
            saveDB.Gold -= 5;
      
            var playerCardsId = saveDB.Decks.SelectMany(a => a.SaveDeckEntries.Select(b => b.CardId)).ToList();

            var message = "Booster oppened successfully";

            if (saveDB.AllCardsCollectedTrophy == false && allCardsIdDB.All(a => playerCardsId.Contains(a)) == true)
            {
                message += " . . .All cards have been collect, a new trophy was unlocked!";
                saveDB.AllCardsCollectedTrophy = true;
            }

            await this._daoDbcontext.SaveChangesAsync();

            return (null, message);
        }
    }
}
