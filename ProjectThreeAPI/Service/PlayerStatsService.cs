using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using Microsoft.EntityFrameworkCore;

namespace MedievalAutoBattler.Service
{
    public class PlayerStatsService
    {
        private readonly ApplicationDbContext _daoDbContext;
        public PlayerStatsService(ApplicationDbContext daoDbContext)
        {
            this._daoDbContext = daoDbContext;
        }

        public async Task<(PlayerStatsReadResponse?, string)> Read(PlayerStatsReadRequest request)
        {
            if(request.SaveId == 0)
            {
                return (null, "Error: invalid SaveId");
            }

            var saveDB = await this._daoDbContext.Saves
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(a => a.Id == request.SaveId && a.IsDeleted == false);

            if (saveDB == null)
            {
                return (null, "Error: save not found");
            }

            var content = new PlayerStatsReadResponse
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

            return (content, "Read successfull");
        }
    }
}
