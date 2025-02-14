using MedievalAutoBattler.Models.Dtos.Request.Players;
using MedievalAutoBattler.Models.Dtos.Response.Players;
using Microsoft.EntityFrameworkCore;

namespace MedievalAutoBattler.Service.Players
{
    public class PlayerCardsService
    {
        public readonly ApplicationDbContext _daoDbContext;
        public PlayerCardsService(ApplicationDbContext daoDbContext)
        {
            _daoDbContext = daoDbContext;
        }

        public async Task<(List<PlayersGetCardsResponse>?, string)> Get(PlayersGetCardsRequest request)
        {
            if (request.SaveId <= 0)
            {
                return (null, "Error: invalid SaveId");
            }

            var collection = await _daoDbContext
                                       .SaveCardEntries
                                       .Where(a => a.Save.Id == request.SaveId)
                                       .Select(a => a.Card)
                                       .ToListAsync();

            if (collection == null || collection.Count == 0)
            {
                return (null, "Error: no cards found for this save.");
            }

            var content = collection
                .GroupBy(card => card.Id)
                .Select(group => new PlayersGetCardsResponse
                {
                    Card = group.First(),
                    Count = group.Count()
                })
                .OrderByDescending(a => a.Card.Level)
                .ToList();

            return (content, "Player's card collection read successfully");
        }
    }
}