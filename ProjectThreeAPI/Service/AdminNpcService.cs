using Microsoft.EntityFrameworkCore;
using ProjectThreeAPI.Models.Dtos.Request;
using ProjectThreeAPI.Models.Dtos.Response;
using ProjectThreeAPI.Models.Entities;
using ProjectThreeAPI.Models.Enums;
using ProjectThreeAPI.Utilities;

namespace ProjectThreeAPI.Service
{
    public class AdminNpcService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public AdminNpcService(ApplicationDbContext daoDBcontext)
        {
            this._daoDbContext = daoDBcontext;
        }

        public async Task<(NpcCreateAdminResponse,string)> Create(NpcCreateAdminRequest npc)
        {
            var (isValid, message) = this.CreateIsValid(npc);

            if (isValid == false)
            {
                return (null,message);
            }

            var exists = await this._daoDbContext
                .Npcs
                .Where(a => a.Name == npc.Name && a.IsDeleted == false)
                .AnyAsync();
            if (exists == true)
            {
                return (null,$"Error: this NPC already exists - {npc.Name}");
            }

            var newNpc = new Npc
            {
                Name = npc.Name,
                Description = npc.Description,
                Hand = npc.Hand,
                Level = Helper.GetNpcLevel(npc),
                IsDeleted = false,
            };

            _daoDbContext.Add(newNpc);

            await _daoDbContext.SaveChangesAsync();

            var result = new NpcCreateAdminResponse
            {
                Id = newNpc.Id,
                Name = newNpc.Name,
                Description = newNpc.Description,
                Hand = newNpc.Hand,
                Level = newNpc.Level,
            };

            return (result,"Create action successful");
        }
        public (bool, string) CreateIsValid(NpcCreateAdminRequest npc)
        {
            if (npc == null)
            {
                return (false, "No information was provided");
            }

            if (string.IsNullOrEmpty(npc.Name) == true)
            {
                return (false, "An NPC's name is mandatory");
            }

            if (string.IsNullOrEmpty(npc.Description) == true)
            {
                return (false, "An NPC's description is mandatory");
            }

            if (npc.Hand.Count == null || npc.Hand.Count != 5)
            {
                return (false, "An NPC's hand cannot be empty, it must have 5 cards");
            }

            return (true, String.Empty);
        }

        //public async Task<List<NpcReadAdminResponse>> Read()
        //{
        //    return await this._daoDbContext
        //        .Npcs
        //        .AsNoTracking()
        //        .Where(a => a.IsDeleted == false)
        //        .Select(a => new NpcReadAdminResponse
        //        {
        //            Id = a.Id,
        //            Name = a.Name,
        //            Power = a.Power,
        //            UpperHand = a.UpperHand,
        //            Level = a.Level,
        //            Type = a.Type,
        //        })
        //        .ToListAsync();
        //}

        //public async Task<string> Update(NpcUpdateAdminRequest npc)
        //{
        //    var (isValid, message) = this.UpdateIsValid(npc);

        //    if (isValid == false)
        //    {
        //        return message;
        //    }

        //    var npcDb = await _daoDbContext
        //        .Npcs
        //        .Where(a => a.Id == npc.Id && a.IsDeleted == false)
        //        .FirstOrDefaultAsync();


        //    if (npcDb == null)
        //    {
        //        return $"Npc not found: {npc.Name}";
        //    }

        //    npcDb.Name = npc.Name;
        //    npcDb.Power = npc.Power;
        //    npcDb.UpperHand = npc.UpperHand;
        //    npcDb.Type = npc.Type;

        //    await _daoDbContext.SaveChangesAsync();

        //    return "Update action successful";
        //}

        //private (bool, string) UpdateIsValid(NpcUpdateAdminRequest npc)
        //{
        //    if (npc == null)
        //    {
        //        return (false, "No information provided");
        //    }

        //    if (string.IsNullOrEmpty(npc.Name) == true)
        //    {
        //        return (false, "Npcs name is mandatory");
        //    }

        //    if (npc.Power == null || npc.Power < 0 || npc.Power > 9)
        //    {
        //        return (false, "Npcs power must be between 0 and 9");
        //    }

        //    if (npc.UpperHand == null || npc.UpperHand < 0 || npc.UpperHand > 9)
        //    {
        //        return (false, "Npcs upper hand must be between 0 and 9");
        //    }

        //    if (Enum.IsDefined(typeof(NpcType), npc.Type) == false)
        //    {
        //        return (false, "Invalid npc type. Type must be None (0), Archer (1), Calvary (2), or Spearman (3).");
        //    }

        //    return (true, String.Empty);
        //}

        //public async Task<string> Delete(int id)
        //{
        //    if (id == null || id <= 0)
        //    {
        //        return $"Error: Invalid Npc ID, ID cannot be empty or equal/lesser than 0";
        //    }

        //    var exists = await this._daoDbContext
        //        .Npcs
        //        .Where(a => a.Id == id && a.IsDeleted == false)
        //        .AnyAsync();

        //    if (exists == false)
        //    {
        //        return $"Error: Invalid Npc ID, the npc does not exist or is already deleted";
        //    }

        //    await this._daoDbContext
        //       .Npcs
        //       .Where(u => u.Id == id)
        //       .ExecuteUpdateAsync(b => b.SetProperty(u => u.IsDeleted, true));

        //    return "Delete action successful";
        //}
    }
}
