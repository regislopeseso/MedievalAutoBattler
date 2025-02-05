using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Entities;
using Microsoft.EntityFrameworkCore;
using ProjectThreeAPI.Migrations;
using ProjectThreeAPI.Models.Dtos.Request;
using ProjectThreeAPI.Models.Entities;
using System.Reflection;

namespace MedievalAutoBattler.Service
{
    public class PlayerDecksService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public PlayerDecksService(ApplicationDbContext daoDbContext)
        {
            _daoDbContext = daoDbContext;
        }

        public async Task<string> Create(PlayerDecksCreateRequest deck)
        {
            var (isValid, message) = this.CreateIsValid(deck);
            if (isValid == false)
            {
                return message;
            }

            var saveDB = await _daoDbContext.Saves
                .Include(b => b.Decks)
                .Where(a => a.Id == deck.SaveId)
                .FirstOrDefaultAsync();
            if (saveDB == null)
            {
                return "Error: invalid save ID";
            }

            var (newSaveDeckEntries, ErrorMessage) = await this.GetNewDeck(deck.CardIds);
            if (newSaveDeckEntries == null || newSaveDeckEntries.Count != 5)
            {
                return ErrorMessage;
            }

            var newDeck = new Deck
            {
                Name = deck.Name,
                SaveDeckEntries = newSaveDeckEntries
            };            

            saveDB.Decks.Add(newDeck);
            await this._daoDbContext.SaveChangesAsync();

            return "Create Successful";
        }        

        private (bool, string) CreateIsValid(PlayerDecksCreateRequest deck)
        {
            if (deck == null)
            {
                return (false, "Error: no information was provided for creating a new Deck");
            }

            if (string.IsNullOrEmpty(deck.Name) == true)
            {
                return (false, "Error: the Deck's name is mandatory");
            }

            if (deck.SaveId <= 0)
            {
                return (false, "Error: saveId invalid");
            }

            if (deck.CardIds == null || deck.CardIds.Count == 0 || deck.CardIds.Count != 5)
            {
                return (false, "Error: the NPC's deck can neither be empty nor contain fewer or more than 5 cards");
            }

            return (true, String.Empty);
        }

        public async Task<string> Update(PlayerDecksUpdateRequest deck)
        {
            var deckDB = await this._daoDbContext.Decks
                .Include(a => a.SaveDeckEntries)
                .ThenInclude(a => a.Card)
                .FirstOrDefaultAsync(a => a.Id == deck.Id);

            if (deckDB == null)
            {
                return $"Error: Deck not found. Invalid DeckId";
            }              

            var oldCardIds = deckDB.SaveDeckEntries.Select(a => a.Id).ToList();
            await this._daoDbContext.SaveDeckEntries
                .Where(a => oldCardIds.Contains(a.CardId) && a.Deck.Id == deck.Id)
                .ExecuteDeleteAsync();


            var (newSaveDeckEntries, ErrorMessage) = await this.GetNewDeck(deck.CardIds);
            if(newSaveDeckEntries == null || newSaveDeckEntries.Count != 5)
            {
                return ErrorMessage;
            }

            deckDB.Name = deck.Name;
            deckDB.SaveDeckEntries = newSaveDeckEntries;

            await _daoDbContext.SaveChangesAsync();

            return "Update successful";
        }

        internal async Task<string> Delete(int deckId)
        {
            if (deckId <= 0)
            {
                return $"Error: Invalid Deck ID, ID cannot be empty or equal/lesser than 0";
            }

            var exists = await this._daoDbContext
                .Decks
                .AnyAsync(a => a.Id == deckId && a.IsDeleted == false);

            if (exists == false)
            {
                return $"Error: Invalid Deck ID, the npc does not exist or is already deleted";
            }

            await this._daoDbContext.Decks
                .Where(a => a.Id == deckId)
                .ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

            return "Delete successful";
        }

        private async Task<(List<SaveDeckEntry>, string)> GetNewDeck(List<int> cardIds)
        {
            var cardsDB = await this._daoDbContext
                .Cards
                .Where(a => cardIds.Contains(a.Id))
                .ToListAsync();
            if (cardsDB == null || cardsDB.Count == 0)
            {
                return ([], "Error: invalid card Ids");
            }

            var uniqueCardIds = cardIds.Distinct().ToList().Count;
            if (uniqueCardIds != cardsDB.Count)
            {
                var notFoundIds = cardIds.Distinct().ToList().Except(cardsDB.Select(a => a.Id).ToList());
                return ([], $"Error: invalid cardId: {string.Join(" ,", notFoundIds)}");
            }

            var deckEntries = new List<SaveDeckEntry>();

            foreach (var id in cardIds)
            {
                var newCard = cardsDB.Where(a => a.Id == id).FirstOrDefault();

                if (newCard != null)
                {
                    deckEntries.Add(new SaveDeckEntry
                    {
                        Card = newCard,
                    });
                }
            }

            return (deckEntries, string.Empty);
        }
    }
}