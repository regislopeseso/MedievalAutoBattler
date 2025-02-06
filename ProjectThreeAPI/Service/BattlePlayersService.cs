using MedievalAutoBattler.Models.Dtos.Request;
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
            var (isValid, message) = this.CreateIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var battleDB = await this._daoDbContext
                                     .Battles
                                     .Include(a => a.Save)
                                     .FirstOrDefaultAsync(a => a.Id == request.BattleId);
            if (battleDB == null)
            {
                return (null, "Error: battle not found");
            }

            var deckDB = await this._daoDbContext
                                             .Saves
                                             .Where(a => a.Id == battleDB.Save.Id)
                                             .Select(a => a.Decks.Where(b => b.Id == request.DeckId).FirstOrDefault())
                                             .FirstOrDefaultAsync();
            if (deckDB == null)
            {
                return (null, "Error: deck not found");
            }

            battleDB.PlayerDeck = deckDB;

            //var deck = await this._daoDbContext
            //                         .Battles
            //                         .Where(a => a.Id == request.BattleId)
            //                         .Include(a => a.Save)
            //                         .ThenInclude(b => b.Decks)                                 
            //                         .Select(a => a.Save.Decks.Where(b => b.Id == request.DeckId).FirstOrDefault())
            //                         .FirstOrDefaultAsync();

            this._daoDbContext.SaveChanges();

            return (null, "Deck chosen successfully");
        }
        public (bool, string) CreateIsValid(BattlePlayersCreateRequest request)
        {
            if (request == null)
            {
                return (false, "Error: no information provided");
            }           

            if (request.BattleId <= 0)
            {
                return (false, "Error: invalid BattleId");
            }

            if (request.DeckId <= 0)
            {
                return (false, "Error: invalid DeckId");
            }

            return (true, String.Empty);
        }
    }
}
