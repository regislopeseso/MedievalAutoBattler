using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Entities;
using Microsoft.EntityFrameworkCore;
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
                .ThenInclude(a => a.DeckEntries)
                .Where(a => a.Id == deck.SaveId)
                .FirstOrDefaultAsync();

            var cardsDB = await this._daoDbContext
                .Cards
                .Where(a => deck.CardIds.Contains(a.Id))
                .ToListAsync();
            if (cardsDB == null || cardsDB.Count == 0)
            {
                return "Error: invalid card Ids";
            }

            var a = deck.CardIds.Distinct().ToList().Count;
            if (a != cardsDB.Count)
            {
                var mistake = deck.CardIds.Distinct().ToList().Except(cardsDB.Select(a => a.Id).ToList());
                return $"Error: invalid cardId: {string.Join(" ,", mistake)}";
            }

            var deckEntries = new List<SaveDeckEntry>();

            foreach (var id in deck.CardIds)
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

            var newDeck = new Deck
            {
                Name = deck.Name,
                DeckEntries = deckEntries
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
    }
}