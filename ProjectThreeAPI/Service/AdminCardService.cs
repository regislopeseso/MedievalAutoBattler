using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly ApplicationDbContext _daoDBcontext;

        public AdminCardService(ApplicationDbContext daoDBcontext)
        {
            this._daoDBcontext = daoDBcontext;
        }

        public async Task<(CardCreateAdminResponse, string)> Create(CardCreateAdminRequest card)
        {
            var (isValid, message) = this.CreateIsValid(card);

            if (isValid == false)
            {
                return (null, message);
            }

            var exists = await this._daoDBcontext
                .Cards
                .Where(a => a.Name == card.Name && a.IsDeleted == false)
                .AnyAsync();
            if (exists == true)
            {
                return (null, $"Erro: a carta já existe - {card.Name}");
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

            _daoDBcontext.Add(newCard);

            await _daoDBcontext.SaveChangesAsync();

            var response = new CardCreateAdminResponse
            {
                Id = newCard.Id,
                Name = newCard.Name,
                Power = newCard.Power,
                UpperHand = newCard.UpperHand,
                Type = newCard.Type,
            };

            return (response, "Create action successful");
        }
        public (bool, string) CreateIsValid(CardCreateAdminRequest card)
        {
            if (card == null)
            {
                return (false, "No information provided");
            }

            if (string.IsNullOrEmpty(card.Name) == true)
            {
                return (false, "Cards name is mandatory");
            }

            if (card.Power == null || card.Power < 0 || card.Power > 9)
            {
                return (false, "Cards power must be between 0 and 9");
            }

            if (card.UpperHand == null || card.UpperHand < 0 || card.UpperHand > 9)
            {
                return (false, "Cards upper hand must be between 0 and 9");
            }

            if (Enum.IsDefined(typeof(CardType), card.Type) == false)
            {
                return (false, "Invalid card type. Type must be None (0), Archer (1), Calvary (2), or Spearman (3).");
            }

            return (true, String.Empty);
        }

        public async Task<List<CardReadAdminResponse>> Read()
        {
            return await this._daoDBcontext
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

        public async Task<string> Update(CardUpdateAdminRequest card)
        {
            var (isValid, message) = this.UpdateIsValid(card);

            if (isValid == false)
            {
                return message;
            }

            var cardDb = await _daoDBcontext
                .Cards
                .Where(a => a.Id == card.Id && a.IsDeleted == false)
                .FirstOrDefaultAsync();


            if (cardDb == null)
            {
                return $"Card not found: {card.Name}";
            }

            cardDb.Name = card.Name;
            cardDb.Power = card.Power;
            cardDb.UpperHand = card.UpperHand;
            cardDb.Type = card.Type;

            await _daoDBcontext.SaveChangesAsync();

            return "Update action successful";
        }

        private (bool, string) UpdateIsValid(CardUpdateAdminRequest card)
        {
            if (card == null)
            {
                return (false, "No information provided");
            }

            if (string.IsNullOrEmpty(card.Name) == true)
            {
                return (false, "Cards name is mandatory");
            }

            if (card.Power == null || card.Power < 0 || card.Power > 9)
            {
                return (false, "Cards power must be between 0 and 9");
            }

            if (card.UpperHand == null || card.UpperHand < 0 || card.UpperHand > 9)
            {
                return (false, "Cards upper hand must be between 0 and 9");
            }

            if (Enum.IsDefined(typeof(CardType), card.Type) == false)
            {
                return (false, "Invalid card type. Type must be None (0), Archer (1), Calvary (2), or Spearman (3).");
            }

            return (true, String.Empty);
        }

        public async Task<string> Delete(CardDeleteAdminRequest card)
        {
            if (card.Id == null || card.Id <= 0)
            {
                return $"Error: Invalid Card ID, ID cannot be empty or equal/lesser than 0";
            }

            var exists = await this._daoDBcontext
                .Cards
                .Where(a => a.Id == card.Id && a.IsDeleted == false)
                .AnyAsync();

            if (exists == false)
            {
                return $"Error: Invalid Card ID, the card does not exist or is already deleted";
            }

            await this._daoDBcontext
               .Cards
               .Where(u => u.Id == card.Id)
               .ExecuteUpdateAsync(b => b.SetProperty(u => u.IsDeleted, true));

            return "Delete action successful";
        }
    }
}
