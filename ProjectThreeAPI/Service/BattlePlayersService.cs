﻿using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedievalAutoBattler.Service
{
    public class BattlePlayersService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public BattlePlayersService(ApplicationDbContext daoDbContext)
        {
            this._daoDbContext = daoDbContext;
        }

        public async Task<(BattlePlayersCreateResponse?, string)> Create(BattlePlayersCreateRequest request)
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

            var newMatch = new Battle
            {
                Save = saveDB,
            };

            this._daoDbContext.Add(newMatch);

            await this._daoDbContext.SaveChangesAsync();

            return (null, "New battle started");
        }
    }
}
