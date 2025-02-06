using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using Microsoft.EntityFrameworkCore;
using MedievalAutoBattler.Models.Entities;

namespace MedievalAutoBattler.Service
{
    public class PlayerCardsService
    {
        public readonly ApplicationDbContext _daoDbContext;
        public PlayerCardsService(ApplicationDbContext daoDbContext)
        {
            this._daoDbContext = daoDbContext;
        }

        public async Task<(List<PlayerCardsReadResponse>?, string)> Read(PlayerCardsReadRequest request)
        {
            if (request.SaveId <= 0)
            {
                return (null, "Error: invalid SaveId");
            }


            var collection = await this._daoDbContext.Decks
                                       .Where(a => a.Save.Id == request.SaveId)
                                       .SelectMany(a => a.SaveDeckEntries)
                                       .Select(a => a.Card)               
                                       .ToListAsync();

            if (collection == null || collection.Count == 0)
            {
                return (null, "Error: no cards found for this save.");
            }

            var content = collection
                .GroupBy(card => card.Id)
                .Select(group => new PlayerCardsReadResponse
                {
                    Card = group.First(),
                    Count = group.Count()
                })
                .ToList();

            return (content, "Read successful");
        }
    }
}