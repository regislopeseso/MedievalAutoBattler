using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedievalAutoBattler.Service
{
    public class BattlePlayersService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public BattlePlayersService(ApplicationDbContext daoDbContext)
        {
            _daoDbContext = daoDbContext;
        }

        public async Task<string> Create(BattlePlayersCreateRequest newBattle)
        {
            if (newBattle.SaveId <= 0)
            {
                return "Error: invalid Save Id";
            }

            var saveDB = await this._daoDbContext
                .Saves
                .FirstOrDefaultAsync(a => a.Id == newBattle.SaveId);

            if (saveDB == null)
            {
                return "Error: save not found.";
            }

            var newMatch = new Battle
            {
                Save = saveDB,
            };

            _daoDbContext.Add(newMatch);

            await _daoDbContext.SaveChangesAsync();

            return "New battle started";
        }
    }
}
