using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedievalAutoBattler.Service
{
    public class PlayerDecksService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public PlayerDecksService(ApplicationDbContext daoDbContext)
        {
            this._daoDbContext = daoDbContext;
        }

        public async Task<(PlayerDecksCreateResponse?, string)> Create(PlayerDecksCreateRequest request)
        {
            var (isValid, message) = this.CreateIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var saveDB = await this._daoDbContext
                                   .Saves
                                   .Include(b => b.Decks)
                                   .FirstOrDefaultAsync(a => a.Id == request.SaveId);

            if (saveDB == null)
            {
                return (null, "Error: invalid SaveId");
            }

            var (newSaveDeckEntries, ErrorMessage) = await this.GetNewDeck(request.CardIds);

            if (newSaveDeckEntries == null || newSaveDeckEntries.Count != 5)
            {
                return (null, ErrorMessage);
            }

            var newDeck = new Deck
            {
                Name = request.Name,
                SaveDeckEntries = newSaveDeckEntries
            };

            saveDB.Decks.Add(newDeck);

            await this._daoDbContext.SaveChangesAsync();

            return (null, "Create Successful");
        }

        private (bool, string) CreateIsValid(PlayerDecksCreateRequest request)
        {
            if (request == null)
            {
                return (false, "Error: no information was provided for creating a new Deck");
            }

            if (string.IsNullOrEmpty(request.Name) == true)
            {
                return (false, "Error: the deck's name is mandatory");
            }

            if (request.SaveId <= 0)
            {
                return (false, "Error: SaveId invalid");
            }

            if (request.CardIds == null || request.CardIds.Count == 0 || request.CardIds.Count != 5)
            {
                return (false, "Error: the NPC's deck can neither be empty nor contain fewer or more than 5 cards");
            }

            return (true, String.Empty);
        }

        public async Task<(PlayerDecksUpdateResponse?, string)> Update(PlayerDecksUpdateRequest request)
        {
            var (isValid, message) = this.UpdateIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var deckDB = await this._daoDbContext
                                   .Decks
                                   .Include(a => a.SaveDeckEntries)
                                   .ThenInclude(a => a.Card)
                                   .FirstOrDefaultAsync(a => a.Id == request.DeckId);

            if (deckDB == null)
            {
                return (null, $"Error: deck not found. Invalid DeckId");
            }

            var oldCardIds = deckDB.SaveDeckEntries.Select(a => a.Id).ToList();

            await this._daoDbContext
                      .SaveDeckEntries
                      .Where(a => oldCardIds.Contains(a.CardId) && a.Deck.Id == request.DeckId)
                      .ExecuteDeleteAsync();


            var (newSaveDeckEntries, ErrorMessage) = await this.GetNewDeck(request.CardIds);

            if (newSaveDeckEntries == null || newSaveDeckEntries.Count != 5)
            {
                return (null, ErrorMessage);
            }

            deckDB.Name = request.Name;
            deckDB.SaveDeckEntries = newSaveDeckEntries;

            await this._daoDbContext.SaveChangesAsync();

            return (null, "Update successful");
        }

        private (bool, string) UpdateIsValid(PlayerDecksUpdateRequest request)
        {
            if (request == null)
            {
                return (false, "Error: no information was provided for updating a deck");
            }

            if (string.IsNullOrEmpty(request.Name) == true)
            {
                return (false, "Error: the deck's name is mandatory");
            }       

            if (request.CardIds == null || request.CardIds.Count == 0 || request.CardIds.Count != 5)
            {
                return (false, "Error: the NPC's deck can neither be empty nor contain fewer or more than 5 cards");
            }

            return (true, String.Empty);
        }

        internal async Task<(PlayerDecksDeleteResponse?, string)> Delete(PlayerDecksDeleteRequest request)
        {
            if (request.DeckId <= 0)
            {
                return (null, $"Error: Invalid Deck ID, ID cannot be empty or equal/lesser than 0");
            }

            var exists = await this._daoDbContext
                                   .Decks
                                   .AnyAsync(a => a.Id == request.DeckId && a.IsDeleted == false);

            if (exists == false)
            {
                return (null, $"Error: Invalid Deck ID, the npc does not exist or is already deleted");
            }

            await this._daoDbContext
                      .Decks
                      .Where(a => a.Id == request.DeckId)
                      .ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

            return (null, "Delete successful");
        }

        private async Task<(List<SaveDeckEntry>?, string)> GetNewDeck(List<int> cardIds)
        {
            var cardsDB = await this._daoDbContext
                                    .Cards
                                    .Where(a => cardIds.Contains(a.Id))
                                    .ToListAsync();

            if (cardsDB == null || cardsDB.Count == 0)
            {
                return (null, "Error: invalid cardIds");
            }

            var uniqueCardIds = cardIds.Distinct().ToList().Count;

            if (uniqueCardIds != cardsDB.Count)
            {
                var notFoundIds = cardIds.Distinct()
                                         .ToList()
                                         .Except(cardsDB.Select(a => a.Id).ToList());

                return (null, $"Error: invalid cardId: {string.Join(", ", notFoundIds)}");
            }

            var saveDeckEntries = new List<SaveDeckEntry>();

            foreach (var id in cardIds)
            {
                var newCard = cardsDB.FirstOrDefault(a => a.Id == id);

                if (newCard != null)
                {
                    saveDeckEntries.Add(new SaveDeckEntry
                    {
                        Card = newCard,
                    });
                }
            }

            return (saveDeckEntries, string.Empty);
        }
    }
}