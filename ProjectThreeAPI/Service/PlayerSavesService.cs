using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Entities;
using Microsoft.EntityFrameworkCore;
using ProjectThreeAPI.Models.Entities;

namespace MedievalAutoBattler.Service
{
    public class PlayerSavesService
    {
        public readonly ApplicationDbContext _daoDbContext;

        public PlayerSavesService(ApplicationDbContext daoDbContext)
        {
            this._daoDbContext = daoDbContext;
        }

        public async Task<string> Create(PlayerSavesCreateRequest save)
        {
            var (isValid, message) = this.CreateIsValid(save);
            if (isValid == false)
            {
                return message;
            }         

            var random = new Random();
            var newDeck = await this._daoDbContext.Cards
                .Where(a => (a.Power + a.UpperHand) < 5)
                .Take(5)
                .ToListAsync();

            var newSave = new Save
            {
                Name = save.Name,
                PlayerLevel = 0,
                Gold = 0,
                CountMatches = 0,
                CountVictories = 0,
                CountDefeats = 0,
                CountBoosters = 0,
                Deck = await GetNewDeck(),
                IsDeleted = false
            };

            this._daoDbContext.Add(newSave);
            await this._daoDbContext.SaveChangesAsync();

            return "Create successful";
        }

        private (bool, string) CreateIsValid(PlayerSavesCreateRequest save)
        {
            if (save == null)
            {
                return (false, "Error: no information provided");
            }

            if (string.IsNullOrEmpty(save.Name) == true)
            {
                return (false, "Error: a Name must be provided");
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
                return null;
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
            return newDeck;
        }
    }
}
