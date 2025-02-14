using MedievalAutoBattler.Models.Dtos.Request.Players;
using MedievalAutoBattler.Models.Dtos.Response.Players;
using MedievalAutoBattler.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MedievalAutoBattler.Service.Players
{
    public class PlayersBoostersService
    {
        private readonly ApplicationDbContext _daoDbcontext;
        public PlayersBoostersService(ApplicationDbContext daoDbcontext)
        {
            _daoDbcontext = daoDbcontext;
        }

        public async Task<(List<PlayersOpenBoosterResponse>?, string)> Open(PlayersOpenBoosterRequest request)
        {
            if (request.SaveId <= 0)
            {
                return (null, "Error: invalid SaveId");
            }

            var random = new Random();

            var saveDB = await _daoDbcontext
                                   .Saves
                                   .Include(a => a.SaveCardEntries)
                                   .FirstOrDefaultAsync(a => a.IsDeleted == false && a.Id == request.SaveId);

            if (saveDB == null)
            {
                return (null, "Error: save not found");
            }
            if (saveDB.Gold < 5)
            {
                return (null, "Error: not enough gold pieces");
            }

            // Criar um HashSet para armazenar os IDs já salvos e melhorar a performance
            var playerSaveCardEntries = saveDB.SaveCardEntries
                                        .GroupBy(a => a.CardId)
                                        .ToDictionary(cardId => cardId.Key, cardCount => cardCount.Count());

            var cardsDB = await _daoDbcontext
                                .Cards
                                .ToListAsync();

            if (cardsDB == null || cardsDB.Count == 0)
            {
                return (null, "Error: no cards available");
            }

            var booster = cardsDB.OrderBy(a => random.Next()).Take(3).ToList();

            if (booster == null || booster.Count == 0)
            {
                return (null, "Error: no booster available");
            }

            var content = new List<PlayersOpenBoosterResponse>();

            foreach (var card in booster.OrderByDescending(a => a.Id))
            {
                content.Add(new PlayersOpenBoosterResponse
                {
                    CardId = card.Id,
                    CardName = card.Name,
                    CardType = card.Type,
                    CardPower = card.Power,
                    CardUpperHand = card.UpperHand,
                });

                playerSaveCardEntries.TryGetValue(card.Id, out int currentCount);

                if (currentCount < 5)
                {
                    saveDB.SaveCardEntries.Add(new SaveCardEntry
                    {
                        SaveId = saveDB.Id,
                        CardId = card.Id,
                    });

                    playerSaveCardEntries[card.Id] = currentCount + 1;
                }
            }

            saveDB.CountBoosters++;
            saveDB.Gold -= 5;

            var message = "Booster oppened successfully";

            var playerCardsId = saveDB.SaveCardEntries.Select(a => a.CardId).ToList();

            if (saveDB.AllCardsCollectedTrophy == false && cardsDB.Select(a => a.Id).All(cardId => playerCardsId.Contains(cardId)) == true)
            {
                message += " . . .All cards have been collect, a new trophy was unlocked!";
                saveDB.AllCardsCollectedTrophy = true;
            }

            if (saveDB.AllCardsCollectedTrophy == true && cardsDB.Select(a => a.Id).All(cardId => playerCardsId.Contains(cardId)) == false)
            {
                message += " . . . there are new cards available to be collected, therefore you lost your All Cards Collected Trophy!";
                saveDB.AllCardsCollectedTrophy = false;
            }

            await _daoDbcontext.SaveChangesAsync();

            return (content, message);
        }


    }
}

