using Microsoft.EntityFrameworkCore;
using ProjectThreeAPI.Models.Dtos.Request;
using ProjectThreeAPI.Models.Dtos.Response;
using ProjectThreeAPI.Models.Entities;
using ProjectThreeAPI.Models.Enums;
using ProjectThreeAPI.Utilities;

namespace ProjectThreeAPI.Service
{
    public class AdminCardService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public AdminCardService(ApplicationDbContext daoDbContext)
        {
            this._daoDbContext = daoDbContext;
        }

        public async Task<string> Create(AdminCardCreateRequest card)
        {
            var (isValid, message) = this.CreateIsValid(card);

            if (isValid == false)
            {
                return message;
            }

            var exists = await this._daoDbContext
                .Cards
                .Where(a => a.Name == card.Name && a.IsDeleted == false)
                .AnyAsync();
            if (exists == true)
            {
                return $"Error: this card already exists - {card.Name}";
            }

            var newCard = new Card
            {
                Name = card.Name,
                Power = card.Power,
                UpperHand = card.UpperHand,
                Level = Helper.GetCardLevel(card),
                Type = card.Type,
                IsDeleted = false,
            };

            _daoDbContext.Add(newCard);

            await _daoDbContext.SaveChangesAsync();            

            return "Create action successful";
        }
        public (bool, string) CreateIsValid(AdminCardCreateRequest card)
        {
            if (card == null)
            {
                return (false, "Error: No information provided");
            }

            if (string.IsNullOrEmpty(card.Name) == true)
            {
                return (false, "Error: The Card's Name is mandatory");
            }

            if (card.Power == null || card.Power < 0 || card.Power > 9)
            {
                return (false, "Error: The Card's Power must be between 0 and 9");
            }

            if (card.UpperHand == null || card.UpperHand < 0 || card.UpperHand > 9)
            {
                return (false, "Error: The Card's Upper Hand must be between 0 and 9");
            }

            if (Enum.IsDefined(typeof(CardType), card.Type) == false)
            {
                return (false, "Error: Invalid Card Type. Type must be None (0), Archer (1), Calvary (2), or Spearman (3).");
            }

            return (true, String.Empty);
        }

        public async Task<List<CardReadAdminResponse>> Read()
        {
            return await this._daoDbContext
                .Cards
                .AsNoTracking()
                .Where(a => a.IsDeleted == false)
                .Select(a => new CardReadAdminResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Power = a.Power,
                    UpperHand = a.UpperHand,
                    Level = a.Level,
                    Type = a.Type,
                })
                .ToListAsync();
        }

        public async Task<string> Update(AdminCardUpdateRequest card)
        {
            var (isValid, message) = this.UpdateIsValid(card);

            if (isValid == false)
            {
                return message;
            }

            var cardDb = await _daoDbContext
                .Cards
                .Where(a => a.Id == card.Id && a.IsDeleted == false)
                .FirstOrDefaultAsync();


            if (cardDb == null)
            {
                return $"Error: Card not found: {card.Name}";
            }

            cardDb.Name = card.Name;
            cardDb.Power = card.Power;
            cardDb.UpperHand = card.UpperHand;
            cardDb.Type = card.Type;

            await _daoDbContext.SaveChangesAsync();

            return "Update action successful";
        }

        private (bool, string) UpdateIsValid(AdminCardUpdateRequest card)
        {
            if (card == null)
            {
                return (false, "Error: No information provided");
            }

            if (string.IsNullOrEmpty(card.Name) == true)
            {
                return (false, "Error: The Card's Name is mandatory");
            }

            if (card.Power == null || card.Power < 0 || card.Power > 9)
            {
                return (false, "Error: The Card's Power must be between 0 and 9");
            }

            if (card.UpperHand == null || card.UpperHand < 0 || card.UpperHand > 9)
            {
                return (false, "Error: The Card's Upper Hand must be between 0 and 9");
            }

            if (Enum.IsDefined(typeof(CardType), card.Type) == false)
            {
                return (false, "Error: Invalid card type. Type must be None (0), Archer (1), Calvary (2), or Spearman (3).");
            }

            return (true, String.Empty);
        }

        public async Task<string> Delete(int id)
        {
            if (id == null || id <= 0)
            {
                return $"Error: Invalid Card Id. Card Ids cannot be empty, equal to or lesser than 0";
            }

            var exists = await this._daoDbContext
                .Cards
                .Where(a => a.Id == id && a.IsDeleted == false)
                .AnyAsync();

            if (exists == false)
            {
                return $"Error: Invalid Card Id, this Card does not exist or is already deleted";
            }

            await this._daoDbContext
               .Cards
               .Where(u => u.Id == id)
               .ExecuteUpdateAsync(b => b.SetProperty(u => u.IsDeleted, true));

            return "Delete action successful";
        }
    }
}
