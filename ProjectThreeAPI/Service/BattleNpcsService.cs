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

        public async Task<string> Create()
        {
            var random = new Random();

            var npcDB = await this._daoDbContext.Npcs
                .ToListAsync();
    
            var opponent = npcDB[random.Next(npcDB.Count)];

            var newMatch = new Battle
            {
                Npc = opponent,
            };

            return "Opponent chosen successfully";
        }


    }
}