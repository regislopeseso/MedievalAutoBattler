using MedievalAutoBattler.Models.Entities;
using MedievalAutoBattler.Models.Enums;
using Microsoft.EntityFrameworkCore;
using MedievalAutoBattler.Utilities;
using MedievalAutoBattler.Models.Dtos.Request.Admin;
using MedievalAutoBattler.Models.Dtos.Response.Admin;

namespace MedievalAutoBattler.Service.Admin
{
    public class AdminCardsService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public AdminCardsService(ApplicationDbContext daoDbContext)
        {
            _daoDbContext = daoDbContext;
        }

        public async Task<(AdminCardsCreateResponse?, string)> Create(AdminCardsCreateRequest request)
        {
            var (isValid, message) = CreateIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var exists = await _daoDbContext
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

            _daoDbContext.Add(newCard);
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

            if (request.Power < Constants.MinCardPower || request.Power > Constants.MaxCardPower)
            {
                return (false, $"Error: the card's power must be between {Constants.MinCardPower} and {Constants.MaxCardPower}");
            }

            if (request.UpperHand < Constants.MinCardUpperHand || request.UpperHand > Constants.MaxCardUpperHand)
            {
                return (false, $"Error: the card's upper hand value must be between {Constants.MinCardUpperHand} and {Constants.MaxCardUpperHand}");
            }

            if (Enum.IsDefined(request.Type) == false)
            {
                var validTypes = string.Join(", ", Enum.GetValues(typeof(CardType))
                                       .Cast<CardType>()
                                       .Select(cardType => $"{cardType} ({(int)cardType})"));

                return (false, $"Error: invalid card type. The type must be one of the following: {validTypes}");
            }

            return (true, string.Empty);
        }

        public async Task<(AdminCardsCreateResponse?, string)> Seed(AdminCardsCreateRequest_seed request)
        {
            var cardsSeed = new List<Card>();

            ////Optionally adds a "miss" card, which has 0 power, 0 upper hand and no type
            //var miss = new Card
            //{
            //    Name = "Miss",
            //    Type = CardType.None,
            //    IsDeleted = false

            //};
            //cardsSeed.Add(miss);

            foreach (var cardType in new[] { CardType.Archer, CardType.Cavalry, CardType.Spearman })
            {
                for (int power = Constants.MinCardPower; power <= Constants.MaxCardPower; power++)
                {
                    for (int upperHand = Constants.MinCardUpperHand; upperHand <= Constants.MaxCardUpperHand; upperHand++)
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

            _daoDbContext.AddRange(cardsSeed);

            await _daoDbContext.SaveChangesAsync();

            return (null, "Cards seeded successfully");
        }

        public async Task<(List<AdminCardsGetResponse>, string)> GetCards(AdminCardsGetRequest request)
        {
            var contentQueriable = _daoDbContext
                                       .Cards
                                       .AsNoTracking()
                                       .Where(a => a.IsDeleted == false);

            var message = "All cards listed successfully";

            if (request.StartCardId.HasValue && request.EndCardId.HasValue == true)
            {
                contentQueriable = contentQueriable.Where(a => a.Id >= request.StartCardId && a.Id <= request.EndCardId);

                message = "Cards listed successfully";

                if (request.StartCardId != request.EndCardId)
                {
                    message = "Card listed successfully";
                }
            }

            var content = await contentQueriable
                                    .Select(a => new AdminCardsGetResponse
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

            return (content, "All cards listed successfully");
        }

        public async Task<(AdminCardsUpdateResponse?, string)> Update(AdminCardsUpdateRequest request)
        {
            var (isValid, message) = UpdateIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var cardDB = await _daoDbContext
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

            await _daoDbContext.SaveChangesAsync();

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

            if (card.Power < Constants.MinCardPower || card.Power > Constants.MaxCardPower)
            {
                return (false, $"Error: the card's power must be between {Constants.MinCardPower} and {Constants.MaxCardPower}");
            }

            if (card.UpperHand < Constants.MinCardUpperHand || card.UpperHand > Constants.MaxCardUpperHand)
            {
                return (false, $"Error: the card's upper hand value must be between {Constants.MinCardUpperHand} and {Constants.MaxCardUpperHand}");
            }

            if (Enum.IsDefined(card.Type) == false)
            {
                var validTypes = string.Join(", ", Enum.GetValues(typeof(CardType))
                                       .Cast<CardType>()
                                       .Select(cardType => $"{cardType} ({(int)cardType})"));

                return (false, $"Error: invalid card type. The type must be one of the following: {validTypes}");
            }

            return (true, string.Empty);
        }

        public async Task<(AdminCardsDeleteResponse?, string)> Delete(AdminCardsDeleteRequest request)
        {
            if (request.CardId <= 0)
            {
                return (null, $"Error: invalid CardID");
            }

            var exists = await _daoDbContext
                .Cards
                .AnyAsync(a => a.Id == request.CardId && a.IsDeleted == false);

            if (exists == false)
            {
                return (null, $"Error: card not found");
            }

            await _daoDbContext
                      .Cards
                      .Where(a => a.Id == request.CardId)
                      .ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

            return (null, "Card deleted successfully");
        }
    }
}
