using MedievalAutoBattler.Models.Dtos.Request.Players;
using MedievalAutoBattler.Models.Dtos.Response.Players;
using MedievalAutoBattler.Models.Entities;
using MedievalAutoBattler.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MedievalAutoBattler.Service.Players
{
    public class PlayerDecksService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public PlayerDecksService(ApplicationDbContext daoDbContext)
        {
            _daoDbContext = daoDbContext;
        }

        public async Task<(PlayersCreateNewDeckResponse?, string)> Create(PlayersCreateNewDeckRequest request)
        {
            var (isValid, message) = CreateIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var saveDB = await _daoDbContext
                                   .Saves
                                   .Include(b => b.SaveCardEntries)
                                   .Include(a => a.Decks)
                                   .FirstOrDefaultAsync(a => a.Id == request.SaveId);

            if (saveDB == null)
            {
                return (null, "Error: invalid SaveId");
            }

            #region Checking if requested CardIds are available in players collection
            var playerSaveCardEntriesDB = saveDB.SaveCardEntries
                                                .GroupBy(a => a.CardId)
                                                .ToDictionary(cardId => cardId.Key, qty => qty.Count());

            // Group the requested cards to count their occurrences
            var requestedCardCounts = request.CardIds
                                             .GroupBy(cardId => cardId)
                                             .ToDictionary(cardId => cardId.Key, qty => qty.Count());

            // Check if the player has enough of each requested card
            foreach (var (cardId, requestedCount) in requestedCardCounts)
            {
                var cardExists = playerSaveCardEntriesDB.TryGetValue(cardId, out int ownedCount);

                if (cardExists == false)
                {
                    return (null, $"Error:  invalid cardIds {cardId}");
                }

                if (requestedCount > ownedCount)
                {
                    return (null, $"Error: Not enough copies of CardId {cardId}. Requested: {requestedCount}, Owned: {ownedCount}");
                }
            }
            #endregion

            var playerCardIdsDB = saveDB.SaveCardEntries.Select(a => a.CardId).ToList();


            var filteredSaveCardEntries = saveDB.SaveCardEntries
                                                .Where(entry => request.CardIds.Contains(entry.CardId))
                                                .ToList();
            var newDeck = new Deck()
            {
                Name = request.Name
            };

            var newSaveDeckEntries = new List<SaveDeckEntry>();
            foreach (var saveCardEntry in filteredSaveCardEntries)
            {
                newSaveDeckEntries.Add(
                    new SaveDeckEntry
                    {
                        SaveCardEntry = saveCardEntry,
                    });
            }
            newDeck.SaveDeckEntries = newSaveDeckEntries;

            saveDB.Decks.Add(newDeck);

            await _daoDbContext.SaveChangesAsync();

            return (null, "A new deck has been created successfully");
        }

        private (bool, string) CreateIsValid(PlayersCreateNewDeckRequest request)
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

            return (true, string.Empty);
        }

        public async Task<(PlayersEditDeckResponse?, string)> Edit(PlayersEditDeckRequest request)
        {
            var (isValid, message) = UpdateIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var saveDB = await _daoDbContext
                                   .Saves
                                   .Include(a => a.Decks)
                                   .ThenInclude(a => a.SaveDeckEntries)
                                   .Include(a => a.SaveCardEntries)
                                   .FirstOrDefaultAsync(a => a.Decks.Any(deck => deck.Id == request.DeckId));

            if (saveDB == null)
            {
                return (null, $"Error: invalid deckId");
            }

            var deckDB = saveDB.Decks.FirstOrDefault(a => a.Id == request.DeckId);

            if (deckDB == null)
            {
                return (null, "Error: deck not found. Invalid DeckId");
            }

            if (deckDB.IsDeleted == true)
            {
                return (null, "Error: this deck was deleted");
            }

            var oldSaveCardEntriesIds = deckDB.SaveDeckEntries
                                              .Select(a => a.SaveCardEntry.Id)
                                              .ToList();

            #region Checking if requested CardIds are available in players collection
            var playerSaveCardEntriesDB = saveDB.SaveCardEntries
                                                .GroupBy(a => a.CardId)
                                                .ToDictionary(cardId => cardId.Key, qty => qty.Count());

            // Group the requested cards to count their occurrences
            var requestedCardCounts = request.CardIds
                                             .GroupBy(cardId => cardId)
                                             .ToDictionary(cardId => cardId.Key, qty => qty.Count());

            // Check if the player has enough of each requested card
            foreach (var (cardId, requestedCount) in requestedCardCounts)
            {
                var cardExists = playerSaveCardEntriesDB.TryGetValue(cardId, out int ownedCount);

                if (cardExists == false)
                {
                    return (null, $"Error:  invalid cardIds {cardId}");
                }

                if (requestedCount > ownedCount)
                {
                    return (null, $"Error: Not enough copies of CardId {cardId}. Requested: {requestedCount}, Owned: {ownedCount}");
                }
            }
            #endregion       

            var filteredSaveCardEntries = saveDB.SaveCardEntries
                                               .Where(entry => request.CardIds.Contains(entry.CardId))
                                               .ToList();

            deckDB.Name = request.Name;

            var updatedSaveDeckEntries = new List<SaveDeckEntry>();

            for (int i = 0; i < Constants.DeckSize; i++)
            {
                deckDB.SaveDeckEntries[i].SaveCardEntry = filteredSaveCardEntries[i];
            }

            await _daoDbContext.SaveChangesAsync();

            return (null, "Deck updated successfully");
        }

        private (bool, string) UpdateIsValid(PlayersEditDeckRequest request)
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

            return (true, string.Empty);
        }

        internal async Task<(PlayersDeleteDeckResponse?, string)> Delete(PlayersDeleteDeckRequest request)
        {
            if (request.DeckId <= 0)
            {
                return (null, $"Error: Invalid Deck ID, ID cannot be empty or equal/lesser than 0");
            }

            var exists = await _daoDbContext
                                   .Decks
                                   .AnyAsync(a => a.Id == request.DeckId && a.IsDeleted == false);

            if (exists == false)
            {
                return (null, $"Error: Invalid Deck ID, the npc does not exist or is already deleted");
            }

            await _daoDbContext
                      .Decks
                      .Where(a => a.Id == request.DeckId)
                      .ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

            return (null, "Delete deleted successfully");
        }

    }
}