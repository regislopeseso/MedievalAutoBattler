using Microsoft.EntityFrameworkCore;
using ProjectThreeAPI.Migrations;
using ProjectThreeAPI.Models.Dtos.Request;
using ProjectThreeAPI.Models.Dtos.Response;
using ProjectThreeAPI.Models.Entities;

namespace ProjectThreeAPI.Service
{
    public class AdminCardNpcService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public AdminCardNpcService(ApplicationDbContext daoDbContext)
        {
            _daoDbContext = daoDbContext;
        }

        public async Task<(CardNpcCreateResponse, string)> Create(CardNpcCreateRequest cardNpc)
        {
            var (isValid, message) = this.CreateIsValid(cardNpc);

            if (isValid == false)
            {
                return (null, message);
            }

            var newHand = new CardNpcCreateResponse
            {
                CardId = cardNpc.CardId,
                NpcId = cardNpc.NpcId,
            };

            if (newHand == null)
            {
                return (null, "Error: failed to associate ids");
            }

            var card = await _daoDbContext
                .Cards
                .Include(a => a.Npcs)
                .Where(a => a.Id == cardNpc.CardId)
                .FirstOrDefaultAsync();

            var npc = await _daoDbContext
                .Npcs
                .FindAsync(newHand.NpcId);

            card.Npcs.Add(npc);

            await _daoDbContext.SaveChangesAsync();

            var result = newHand;

            return (result, "Create action successful");
        }

        public (bool, string) CreateIsValid(CardNpcCreateRequest cardNpc)
        {
            if (cardNpc == null) 
            {
                return (false, "Error: No information provided.");
            }

            if (cardNpc.CardId <= 0)
            {
                return (false, "Error: Card Id is incorrect.");
            }

            if (cardNpc.NpcId <= 0)
            {
                return (false, "Error: NPC Id is incorrect.");
            }

            return (true, String.Empty);
        }
    }
}
