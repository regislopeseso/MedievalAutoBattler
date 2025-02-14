using MedievalAutoBattler.Models.Dtos.Request.Players;
using MedievalAutoBattler.Models.Dtos.Response.Players;
using Microsoft.EntityFrameworkCore;

namespace MedievalAutoBattler.Service.Players
{
    public class PlayerStatsService
    {
        private readonly ApplicationDbContext _daoDbContext;
        public PlayerStatsService(ApplicationDbContext daoDbContext)
        {
            _daoDbContext = daoDbContext;
        }

        public async Task<(PlayersGetStatsResponse?, string)> Get(PlayersGetStatsRequest request)
        {
            if (request.SaveId == 0)
            {
                return (null, "Error: invalid SaveId");
            }

            var saveDB = await _daoDbContext.Saves
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(a => a.Id == request.SaveId && a.IsDeleted == false);

            if (saveDB == null)
            {
                return (null, "Error: save not found");
            }

            var content = new PlayersGetStatsResponse
            {
                Name = saveDB.Name,
                Gold = saveDB.Gold,
                CountMatches = saveDB.CountMatches,
                CountVictories = saveDB.CountVictories,
                CountDefeats = saveDB.CountDefeats,
                CountBoosters = saveDB.CountBoosters,
                PlayerLevel = saveDB.PlayerLevel,
                AllCardsCollectedTrophy = saveDB.AllCardsCollectedTrophy,
                AllNpcsDefeatedTrophy = saveDB.AllNpcsDefeatedTrophy
            };

            return (content, "Save statistics read successfully");
        }
    }
}
