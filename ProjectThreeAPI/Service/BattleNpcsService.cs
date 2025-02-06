using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Models.Entities;
using Microsoft.EntityFrameworkCore;
using ProjectThreeAPI.Models.Entities;
using System.Text.RegularExpressions;

namespace MedievalAutoBattler.Service
{
    public class BattleNpcsService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public BattleNpcsService(ApplicationDbContext daoDbContext)
        {
            _daoDbContext = daoDbContext;
        }
       
        public async Task<string> Create(BattleNpcsCreateRequest request)
        {
            if(request.BattleId <= 0)
            {
                return "Error: invalid Battle ID";
            }

            var battleDB = await this._daoDbContext
                .Battles
                .FirstOrDefaultAsync(a => a.Id == request.BattleId);

            if (battleDB == null)
            {
                return "Error: battle not found";
            }


            var npcDB = await this._daoDbContext
                .Npcs
                .ToListAsync();    

            var random = new Random();
            var randomOpponent = npcDB[random.Next(npcDB.Count)];

            battleDB.Npc = randomOpponent;        

            await _daoDbContext.SaveChangesAsync();

            return "Random opponent chosen successfully";
        }

        public async Task<(BattleNpcsReadResponse?, string)> Read(int battleId)
        {
            if (battleId <= 0)
            {
                return (null, "Error: invalid Battle Id");
            }

            var npcNameDB = await this._daoDbContext.Battles
                .Where(a => a.Id == battleId)
                .Select(a => a.Npc.Name)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(npcNameDB) == true)
            {
                return (null, "Error: invalid NPC");
            }

            var npcName = new BattleNpcsReadResponse
            {
                Name = npcNameDB
            };

            return (npcName, "Read successful");                
        }     

    }
}