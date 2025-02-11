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

        public async Task<(DeckBoostersCreateResponse?, string)> Create(DeckBoostersCreateRequest request)
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
                                   .ThenInclude(b => b.SaveDeckEntries)
                                   .FirstOrDefaultAsync(a => a.IsDeleted == false && a.Id == request.SaveId);

            if (saveDB == null)
            {
                return (null, "Error: save not found");
            }
            if(saveDB.Gold < 5)
            {
                return (null, "Error: not enough gold pieces");
            }

            var cardsDB = await this._daoDbcontext
                                .Cards
                                .ToListAsync();            
            
            var booster = cardsDB.OrderBy(a => random.Next()).Take(3).ToList();
            
            foreach (var card in booster)
            {
                newSaveDeckEntries.Add(new SaveDeckEntry 
                {
                    Card = card,
                });
            }

            saveDB.Decks.Add(new Deck
            {
                Name = "Booster " + saveDB.CountBoosters + 1,
                SaveDeckEntries = newSaveDeckEntries
            });
            saveDB.CountBoosters++;
            saveDB.Gold -= 5;

            var allCardsIdDB = cardsDB.Select(a => a.Id).ToList();

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
