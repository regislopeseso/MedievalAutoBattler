using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using Microsoft.EntityFrameworkCore;
using ProjectThreeAPI.Models.Dtos.Request;
using ProjectThreeAPI.Models.Dtos.Response;
using ProjectThreeAPI.Models.Entities;
using ProjectThreeAPI.Models.Enums;
using ProjectThreeAPI.Utilities;

namespace ProjectThreeAPI.Service
{
    public class AdminCardsService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public AdminCardsService(ApplicationDbContext daoDbContext)
        {
            this._daoDbContext = daoDbContext;
        }

        public async Task<(AdminCardsCreateResponse?, string)> Create(AdminCardsCreateRequest request)
        {
            var (isValid, message) = this.CreateIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var exists = await this._daoDbContext
                .Cards
                .AnyAsync(a => a.Name == request.Name && a.IsDeleted == false);

            if (exists == true)
            {
                return (null, $"Error: this card already exists - {request.Name}");
            }

            var newCard = new Card
            {
                Name = request.Name,
                Power = request.Power,
                UpperHand = request.UpperHand,
                Level = Helper.GetCardLevel(request.Power, request.UpperHand),
                Type = request.Type,
                IsDeleted = false,
            };

            this._daoDbContext.Add(newCard);
            await _daoDbContext.SaveChangesAsync();

            return (null, "Create action successful");
        }
        public (bool, string) CreateIsValid(AdminCardsCreateRequest request)
        {
            if (request == null)
            {
                return (false, "Error: no information provided");
            }

            if (string.IsNullOrEmpty(request.Name) == true)
            {
                return (false, "Error: the card's name is mandatory");
            }

            if (request.Power < 0 || request.Power > 9)
            {
                return (false, "Error: the card's power must be between 0 and 9");
            }

            if (request.UpperHand < 0 || request.UpperHand > 9)
            {
                return (false, "Error: the card's upper hand value must be between 0 and 9");
            }

            if (Enum.IsDefined(typeof(CardType), request.Type) == false)
            {
                return (false, "Error: invalid vard type. The type must be one of the following: none (0), archer (1), calvary (2) or spearman (3)");
            }

            return (true, String.Empty);
        }

        public async Task<(List<AdminCardsReadResponse>, string)> Read()
        {
            return (await this._daoDbContext
                .Cards
                .AsNoTracking()
                .Where(a => a.IsDeleted == false)
                .Select(a => new AdminCardsReadResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Power = a.Power,
                    UpperHand = a.UpperHand,
                    Level = a.Level,
                    Type = a.Type,
                })
                .OrderBy(a => a.Id)
                .ThenBy(a => a.Name)
                .ToListAsync(), "Read successful");
        }

        public async Task<(AdminCardsUpdateResponse?, string)> Update(AdminCardsUpdateRequest request)
        {
            var (isValid, message) = this.UpdateIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var cardDB = await this._daoDbContext
                .Cards
                .FirstOrDefaultAsync(a => a.Id == request.Id && a.IsDeleted == false);

            if (cardDB == null)
            {
                return (null, $"Error: card not found: {request.Name}");
            }

            cardDB.Name = request.Name;
            cardDB.Power = request.Power;
            cardDB.UpperHand = request.UpperHand;
            cardDB.Type = request.Type;

            await this._daoDbContext.SaveChangesAsync();

            return (null, "Update action successful");
        }

        private (bool, string) UpdateIsValid(AdminCardsUpdateRequest card)
        {
            if (card == null)
            {
                return (false, "Error: no information provided");
            }

            if (string.IsNullOrEmpty(card.Name) == true)
            {
                return (false, "Error: the card's name is mandatory");
            }

            if (card.Power < 0 || card.Power > 9)
            {
                return (false, "Error: the card's power must be between 0 and 9");
            }

            if (card.UpperHand < 0 || card.UpperHand > 9)
            {
                return (false, "Error: the card's upper hand value must be between 0 and 9");
            }

            if (Enum.IsDefined(typeof(CardType), card.Type) == false)
            {
                return (false, "Error: invalid card type. The type must be one of the following: none (0), archer (1), calvary (2) or spearman (3)");
            }

            return (true, String.Empty);
        }

        public async Task<(AdminCardsDeleteResponse?, string)> Delete(AdminCardsDeleteRequest request)
        {
            if (request.CardId <= 0)
            {
                return (null, $"Error: invalid CardID");
            }

            var exists = await this._daoDbContext
                .Cards
                .AnyAsync(a => a.Id == request.CardId && a.IsDeleted == false);

            if (exists == false)
            {
                return (null, $"Error: card not found");
            }

            await this._daoDbContext
               .Cards
               .Where(u => u.Id == request.CardId)
               .ExecuteUpdateAsync(b => b.SetProperty(u => u.IsDeleted, true));

            return (null, "Delete action successful");
        }
    }
}
