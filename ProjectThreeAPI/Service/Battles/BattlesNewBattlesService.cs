using MedievalAutoBattler.Models.Dtos.Request.Battles;
using MedievalAutoBattler.Models.Dtos.Response.Battles;
using MedievalAutoBattler.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedievalAutoBattler.Service.Battles
{
    public class BattlesNewBattlesService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public BattlesNewBattlesService(ApplicationDbContext daoDbContext)
        {
            _daoDbContext = daoDbContext;
        }

        public async Task<(BattlesNewBattleCreateResponse?, string)> Create(BattlesNewBattleCreateRequest request)
        {
            if (request.SaveId <= 0)
            {
                return (null, "Error: invalid Save Id");
            }

            var saveDB = await _daoDbContext
                                   .Saves
                                   .FirstOrDefaultAsync(a => a.Id == request.SaveId);
            if (saveDB == null)
            {
                return (null, "Error: save not found.");
            }

            var random = new Random();

            var validNpcsIdsDB = await _daoDbContext
                                           .Npcs
                                           .Where(a => a.IsDeleted == false && a.Level <= saveDB.PlayerLevel + 1)
                                           .Select(a => new { a.Id, a.Name })
                                           .ToListAsync();

            if (validNpcsIdsDB == null || validNpcsIdsDB.Count == 0)
            {
                return (null, "Error: no valid NPC was found for this match");
            }

            var randomNpc = validNpcsIdsDB.OrderBy(a => random.Next()).FirstOrDefault();

            var newMatch = new Battle
            {
                SaveId = saveDB.Id,
                NpcId = randomNpc.Id
            };

            _daoDbContext.Add(newMatch);

            await _daoDbContext.SaveChangesAsync();

            var content = new BattlesNewBattleCreateResponse
            {
                BattleId = newMatch.Id,
                NpcName = randomNpc.Name
            };

            return (content, "A new battle started successfully");
        }
    }
}
