using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Models.Entities;
using MedievalAutoBattler.Models.Enums;
using Microsoft.EntityFrameworkCore;
using MedievalAutoBattler.Utilities;

namespace MedievalAutoBattler.Service
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

            return (null, "Card created successfully");
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

            if (Enum.IsDefined(request.Type) == false)
            {
                return (false, "Error: invalid vard type. The type must be one of the following: none (0), archer (1), calvary (2) or spearman (3)");
            }

            return (true, String.Empty);
        }

        public async Task<(AdminCardsCreateResponse?, string)> Seed(AdminCardsCreateRequest_seed request)
        {
            var cardsSeed = new List<Card>();

            var miss = new Card
            {
                Name = "Miss",
                Type = CardType.None,
                IsDeleted = false

            };
            cardsSeed.Add(miss);

            foreach (var cardType in new[] { CardType.Archer, CardType.Cavalry, CardType.Spearman })
            {
                for (int power = 0; power < 10; power++)
                {
                    for (int upperHand = 0; upperHand < 10; upperHand++)
                    {
                        var newCard = new Card
                        {
                            Name = cardType.ToString() + " *" + power + "|" + upperHand + "*",
                            Power = power,
                            UpperHand = upperHand,
                            Level = Helper.GetCardLevel(power, upperHand),
                            Type = cardType,
                            IsDeleted = false
                        };
                        cardsSeed.Add(newCard);
                    }
                }
            }

            this._daoDbContext.AddRange(cardsSeed);

            await _daoDbContext.SaveChangesAsync();

            return (null, "Cards seeded successfully");
        }

        public async Task<(List<AdminCardsReadResponse>, string)> Read()
        {
            var content = await this._daoDbContext
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
                                    .ToListAsync();

            return (content, "Cards read successfully");
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

            return (null, "Card updated successfully");
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
                      .Where(a => a.Id == request.CardId)
                      .ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

            return (null, "Card deleted successfully");
        }
    }
}
