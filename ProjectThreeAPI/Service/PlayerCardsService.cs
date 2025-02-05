using MedievalAutoBattler.Models.Dtos.Response;
using Microsoft.EntityFrameworkCore;
using ProjectThreeAPI.Models.Entities;

namespace MedievalAutoBattler.Service
{
    public class PlayerCardsService
    {
        public readonly ApplicationDbContext _daoDbContext;
        public PlayerCardsService(ApplicationDbContext daoDbContext)
        {
            _daoDbContext = daoDbContext;
        }

        public async Task<(List<PlayerCardsReadResponse>?, string)> Read(int saveId)
        {
            if (saveId <= 0)
            {
                return (null, "Error: invalid Id");
            }


            var collection = await this._daoDbContext.Decks
                .Where(a => a.Save.Id == saveId)
                .SelectMany(a => a.DeckEntries)
                .Select(a => a.Card)               
                .ToListAsync();

            if (collection == null || collection.Count == 0)
            {
                return (null, "Error: no cards found for this save.");
            }

            var result = collection
                .GroupBy(card => card.Id)
                .Select(group => new PlayerCardsReadResponse
                {
                    Card = group.First(),
                    Count = group.Count()
                })
                .ToList();

            return (result, "Read successful");
        }
    }
}