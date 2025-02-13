using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Models.Entities;
using MedievalAutoBattler.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MedievalAutoBattler.Service
{
    public class PlayerSavesService
    {
        public readonly ApplicationDbContext _daoDbContext;

        public PlayerSavesService(ApplicationDbContext daoDbContext)
        {
            this._daoDbContext = daoDbContext;
        }

        public async Task<(PlayerSavesCreateResponse?, string)> Create(PlayerSavesCreateRequest request)
        {
            var (isValid, message) = this.CreateIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var newSave = new Save
            {
                Name = request.Name,
            };

            var initialSaveCardEntries = new List<SaveCardEntry>();
            (initialSaveCardEntries, message) = await GetInitialSaveCardEntries(newSave.Id);
            if (initialSaveCardEntries == null || initialSaveCardEntries.Count == 0)
            {
                return (null, message);
            }

            var initialSaveDeckEntries = new List<SaveDeckEntry>();
            (initialSaveDeckEntries, message) = GetInitialSaveDeckEntries(initialSaveCardEntries);
            if (initialSaveDeckEntries == null || initialSaveDeckEntries.Count == 0)
            {
                return (null, message);
            }

            newSave.SaveCardEntries = initialSaveCardEntries;

            newSave.Decks = new List<Deck>()
            {
                new Deck
                {
                    Name = "Initial Deck",
                    SaveDeckEntries = initialSaveDeckEntries
                }
            };



            this._daoDbContext.Add(newSave);

            await this._daoDbContext.SaveChangesAsync();

            var content = new PlayerSavesCreateResponse
            {
                SaveId = newSave.Id,
            };

            return (content, "A new save has been created successfully");
        }

        private (bool, string) CreateIsValid(PlayerSavesCreateRequest request)
        {
            if (request == null)
            {
                return (false, "Error: no information was provided");
            }

            if (string.IsNullOrEmpty(request.Name) == true)
            {
                return (false, "Error: a name must be provided");
            }

            return (true, String.Empty);
        }
        private async Task<(List<SaveCardEntry>?, string)> GetInitialSaveCardEntries(int saveId)
        {
            var validInitialCardsDB = await this._daoDbContext
                                    .Cards
                                    .Where(a => ((a.Power + a.UpperHand) < 5) && (a.IsDeleted == false))
                                    .Select(a => a.Id)
                                    .ToListAsync();

            if (validInitialCardsDB == null || validInitialCardsDB.Count == 0)
            {
                return (null, "Error: no cards having power + upper hand < 5 were found");
            }

            var random = new Random();
            var randomCardIds = new List<int>();

            while (randomCardIds.Count < Constants.DeckSize)
            {
                randomCardIds.Add(validInitialCardsDB[random.Next(validInitialCardsDB.Count)]);
            }

            var initialSaveCardEntries = new List<SaveCardEntry>();

            foreach (var cardId in randomCardIds)
            {
                if (cardId == 0)
                {
                    return (null, "Error: invalid cardId for initial SaveCardEntries");
                }

                initialSaveCardEntries.Add(new SaveCardEntry()
                {
                    SaveId = saveId,
                    CardId = cardId,
                });
            }

            return (initialSaveCardEntries, string.Empty);
        }
        private (List<SaveDeckEntry>?, string) GetInitialSaveDeckEntries(List<SaveCardEntry> initialSaveCardEntries)
        {
            var initialSaveDeckEntries = new List<SaveDeckEntry>();

            foreach (var initialSaveDeckEntry in initialSaveCardEntries)
            {
                initialSaveDeckEntries.Add(new SaveDeckEntry()
                {
                    SaveCardEntry = initialSaveDeckEntry
                });
            }

            if (initialSaveDeckEntries.Count == 0)
            {
                return (null, "Error: invalid cardId for initial deck");
            }

            return (initialSaveDeckEntries, string.Empty);
        }
    }
}
