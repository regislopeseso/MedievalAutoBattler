using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedievalAutoBattler.Service
{
    public class BattleSavesService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public BattleSavesService(ApplicationDbContext daoDbContext)
        {
            this._daoDbContext = daoDbContext;
        }

        public async Task<(BattleSavesCreateResponse?, string)> Create(BattleSavesCreateRequest request)
        {
            if (request.SaveId <= 0)
            {
                return (null, "Error: invalid Save Id");
            }

            var saveDB = await this._daoDbContext
                                   .Saves
                                   .FirstOrDefaultAsync(a => a.Id == request.SaveId);
            if (saveDB == null)
            {
                return (null, "Error: save not found.");
            }

            var random = new Random();

            var validNpcsIdsDB = await this._daoDbContext
                                           .Npcs
                                           .Where(a => a.IsDeleted == false && a.Level <= saveDB.PlayerLevel + 1)
                                           .Select(a =>new {a.Id, a.Name})
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

            this._daoDbContext.Add(newMatch);

            await this._daoDbContext.SaveChangesAsync();

            var content = new BattleSavesCreateResponse
            {
                BattleId = newMatch.Id,
                NpcName = randomNpc.Name
            };

            return (content, "A new battle started successfully");
        }         
    }
}
