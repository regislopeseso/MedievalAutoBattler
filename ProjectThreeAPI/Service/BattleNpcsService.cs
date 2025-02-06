using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using Microsoft.EntityFrameworkCore;

namespace MedievalAutoBattler.Service
{
    public class BattleNpcsService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public BattleNpcsService(ApplicationDbContext daoDbContext)
        {
            this._daoDbContext = daoDbContext;
        }
       
        public async Task<(BattleNpcsCreateResponse?, string)> Create(BattleNpcsCreateRequest request)
        {
            if(request.BattleId <= 0)
            {
                return (null, "Error: invalid Battle ID");
            }

            var battleDB = await this._daoDbContext
                                     .Battles
                                     .FirstOrDefaultAsync(a => a.Id == request.BattleId);

            if (battleDB == null)
            {
                return (null, "Error: battle not found");
            }

            var npcDB = await this._daoDbContext
                                  .Npcs
                                  .ToListAsync();    

            var random = new Random();
            var randomOpponent = npcDB[random.Next(npcDB.Count)];

            battleDB.Npc = randomOpponent;        

            await this._daoDbContext.SaveChangesAsync();

            return (null, "Random opponent chosen successfully");
        }

        public async Task<(BattleNpcsReadResponse?, string)> Read(BattleNpcsReadRequest request)
        {
            if (request.BattleId <= 0)
            {
                return (null, "Error: invalid B=battle Id");
            }

            var npcNameDB = await this._daoDbContext.Battles
                                      .Where(a => a.Id == request.BattleId)
                                      .Select(a => a.Npc.Name)
                                      .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(npcNameDB) == true)
            {
                return (null, "Error: battle not found");
            }

            var content = new BattleNpcsReadResponse
            {
                Name = npcNameDB
            };

            return (content, "Read successful");                
        }    
    }
}