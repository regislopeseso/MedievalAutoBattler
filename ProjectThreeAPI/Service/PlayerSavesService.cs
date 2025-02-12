using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Models.Entities;
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
                Decks = new List<Deck>
                {
                    new Deck
                    {
                        Name = "Starting Deck",
                        SaveDeckEntries = await this.GetNewDeck()
                    }
                },
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
        private async Task<List<SaveDeckEntry>> GetNewDeck()
        {
            var cardsDB = await this._daoDbContext
                                    .Cards
                                    .Where(a => ((a.Power + a.UpperHand) < 5) && (a.IsDeleted == false))
                                    .ToListAsync();

            if (cardsDB == null || cardsDB.Count == 0)
            {
                return [];
            }

            var random = new Random();
            var totalCards = cardsDB.Count;
            var randomCards = new List<Card>();

            while (randomCards.Count < 5)
            {
                randomCards.Add(cardsDB[random.Next(totalCards)]);
            }

            var newDeck = new List<SaveDeckEntry>();

            foreach (var card in randomCards)
            {
                if (card != null)
                {
                    newDeck.Add(new SaveDeckEntry()
                    {
                        Card = card,
                    });
                }
            }

            if(newDeck == null || newDeck.Count == 0)
            {
                return [];
            }

            return newDeck;
        }
    }
}
