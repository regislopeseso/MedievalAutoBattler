using MedievalAutoBattler.Models.Dtos.Response;
using Microsoft.EntityFrameworkCore;

namespace MedievalAutoBattler.Service
{
    public class PlayerStatsService
    {
        private readonly ApplicationDbContext _daoDbContext;
        public PlayerStatsService(ApplicationDbContext daoDbContext)
        {
            _daoDbContext = daoDbContext;
        }

        public async Task<(PlayerStatsReadResponse?, string)> Read(int saveId)
        {
            if(saveId == 0)
            {
                return (null, "Error: informing a valid id is mandatory");
            }

            var saveDB = await this._daoDbContext.Saves
                .AsNoTracking()
                .Where(a => a.Id == saveId && a.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (saveDB == null)
            {
                return (null, "Error: invalid id");
            }

            var stats = new PlayerStatsReadResponse
            {
                Name = saveDB.Name,
                Gold = saveDB.Gold,
                CountMatches = saveDB.CountMatches,
                CountVictories = saveDB.CountVictories,
                CountDefeats = saveDB.CountDefeats,
                CountBoosters = saveDB.CountBoosters,
                PlayerLevel = saveDB.PlayerLevel
            };

            return (stats, "Read successfull");
        }
    }
}
