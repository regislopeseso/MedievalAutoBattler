using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using Microsoft.EntityFrameworkCore;

namespace MedievalAutoBattler.Service
{
    public class BattleResultsService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public BattleResultsService(ApplicationDbContext daoDbContext)
        {
            this._daoDbContext = daoDbContext;
        }

        public async Task<(BattleResultsReadResponse?, string)> Read(BattleResultsReadRequest request)
        {
            var battleDB = await this._daoDbContext
                                 .Battles
                                 .Include(a => a.Npc)
                                 .ThenInclude(b => b.Deck)
                                 .ThenInclude(c => c.Card)
                                 .Include(a => a.Save)
                                 .ThenInclude(b => b.Decks)
                                 .ThenInclude(c => c.SaveDeckEntries)
                                 .ThenInclude(d => d.Card)
                                 .FirstOrDefaultAsync(a => a.Id == request.BattleId);

            var save = battleDB.Save;
            var playerDeck = battleDB.PlayerDeck;
            var playerCardNames = playerDeck.SaveDeckEntries.Select(a => a.Card.Name).ToList();

            var npc = battleDB.Npc;
            var npcDeck = npc.Deck;
            var npcCardNames = npcDeck.Select(a => a.Card.Name).ToList();

            var content = new BattleResultsReadResponse();
            content.NpcCards = npcCardNames;
            content.PlayerCards = playerCardNames;
            //content.Winner = "teste";

            await this._daoDbContext.SaveChangesAsync();

            return (content, "Results read sucessfully");
        }
    }
}
