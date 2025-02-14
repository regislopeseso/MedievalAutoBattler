using MedievalAutoBattler.Models.Entities;
using MedievalAutoBattler.Models.Enums;
using Microsoft.EntityFrameworkCore;
using MedievalAutoBattler.Utilities;
using MedievalAutoBattler.Models.Dtos.Request.Admin;
using MedievalAutoBattler.Models.Dtos.Response.Admin;

namespace MedievalAutoBattler.Service
{
    public class AdminsService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public AdminsService(ApplicationDbContext daoDbContext)
        {
            _daoDbContext = daoDbContext;
        }

        #region Admins Cards Management
        public async Task<(AdminsCreateCardResponse?, string)> CreateCard(AdminsCreateCardRequest request)
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
        public (bool, string) CreateIsValid(AdminsCreateCardRequest request)
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

        public async Task<(AdminsSeedCardsResponse?, string)> SeedCards(AdminsSeedCardsRequest request)
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

        public async Task<(List<AdminsGetCardsResponse>, string)> GetCards(AdminsGetCardsRequest request)
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
                                    .Select(a => new AdminsGetCardsResponse
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

        public async Task<(AdminsEditCardResponse?, string)> EditCards(AdminsEditCardRequest request)
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

        private (bool, string) UpdateIsValid(AdminsEditCardRequest card)
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

        public async Task<(AdminsDeleteCardResponse?, string)> DeleteCards(AdminsDeleteCardRequest request)
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
        #endregion

        #region Admin NPC management
        public async Task<(AdminsCreateNpcResponse?, string)> CreateNpc(AdminsCreateNpcRequest request)
        {
            var (isValid, message) = CreateIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var exists = await _daoDbContext
                                   .Npcs
                                   .AnyAsync(a => a.Name == request.Name && a.IsDeleted == false);

            if (exists == true)
            {
                return (null, $"Error: this NPC already exists - {request.Name}");
            }

            var (newNpcDeckEntries, ErrorMessage) = await GenerateRandomDeck(request.CardIds);

            if (newNpcDeckEntries == null || newNpcDeckEntries.Count != 5)
            {
                return (null, ErrorMessage);
            }

            var newNpc = new Npc
            {
                Name = request.Name,
                Description = request.Description,
                Deck = newNpcDeckEntries,
                Level = Helper.GetNpcLevel(newNpcDeckEntries.Select(a => a.Card.Level).ToList()),
                IsDeleted = false
            };

            _daoDbContext.Add(newNpc);

            await _daoDbContext.SaveChangesAsync();

            return (null, "NPC created successfully");
        }
        public (bool, string) CreateIsValid(AdminsCreateNpcRequest request)
        {
            if (request == null)
            {
                return (false, "Error: no information was provided");
            }

            if (string.IsNullOrWhiteSpace(request.Name) == true)
            {
                return (false, "Error: the NPC's name is mandatory");
            }

            if (string.IsNullOrEmpty(request.Description) == true)
            {
                return (false, "Error: the NPC's description is mandatory");
            }

            if (request.CardIds == null || request.CardIds.Count != Constants.DeckSize)
            {
                return (false, $"Error: the NPC's deck can neither be empty nor contain fewer or more than {Constants.DeckSize} cards");
            }

            return (true, string.Empty);
        }

        public async Task<(AdminsSeedNpcsResponse?, string)> SeedNpcs(AdminsSeedNpcsRequest request)
        {
            var cardsDB = await _daoDbContext
                                    .Cards
                                    .Where(a => a.IsDeleted == false)
                                    .ToListAsync();

            if (cardsDB == null || cardsDB.Count == 0)
            {
                return (null, "Error: cards not found");
            }

            //Se não existir pelo menos uma carta de cada level não é possível fazer o seed dos NPCs.
            var countCardsLvl = cardsDB.GroupBy(a => a.Level).Count();
            if (countCardsLvl < Constants.MaxCardLvl - Constants.MinCardLvl)
            {
                return (null, "Error: not enough card variety for seeding NPCs. The existance of at least one card of each level is mandatory for seeding NPCs");
            }

            var npcsSeed = new List<Npc>();

            for (int level = Constants.MinCardLvl; level <= Constants.MaxCardLvl; level++)
            {
                npcsSeed.AddRange(GenerateRandomNpcs(level, cardsDB));
            }

            if (npcsSeed == null || npcsSeed.Count == 0)
            {
                return (null, "Error: seeding NPCs failed");
            }

            _daoDbContext.AddRange(npcsSeed);

            await _daoDbContext.SaveChangesAsync();

            return (null, "NPCs have been successfully seeded");
        }
        private static List<Npc> GenerateRandomNpcs(int level, List<Card> cardsDB)
        {
            var random = new Random();

            if (level == Constants.MinCardLvl || level == Constants.MaxCardLvl)
            {
                var countBotsLvlZero = 0;
                var countBotsLvlNine = 0;

                // Creating a list that will contain 10 NPCs of cardLvl 0 or cardLvl 9:
                var npcs = new List<Npc>();

                while (npcs.Count < 10)
                {
                    // Filtering all cards having cardLvl iguals to 0 or 9 (currently contains 4 cards npcLvl 0 or 10 npcLvl 9):
                    var cardsFiltered = cardsDB.Where(a => a.Level == level).ToList();

                    // Creating a new list of NpcDeckentries
                    var validNpcDeckEntries = new List<NpcDeckEntry>();
                    for (int countCards = 0; countCards < 5; countCards++)
                    {
                        // Obtaining one random card out of the list of filtered cards
                        var card = cardsFiltered.OrderBy(a => random.Next()).FirstOrDefault();

                        // "Converting" the random card into a new NPC DECK ENTRY and adding it to a new list of valid deck entries:
                        validNpcDeckEntries.Add
                        (
                            new NpcDeckEntry
                            {
                                Card = card
                            }
                        );
                    }

                    // Creating a new npc with 5 cards (npcLvl 0 or npcLvl 9) and adding it to a list of new NPCs:          
                    var npcName = "";
                    var npcDescription = "";
                    var npcLvl = Helper.GetNpcLevel(validNpcDeckEntries.Select(a => a.Card.Level).ToList());
                    if (level == 0)
                    {
                        countBotsLvlZero++;
                        npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlZero;
                        npcDescription = "(0, 0, 0, 0, 0)";
                    }
                    if (level == 9)
                    {
                        countBotsLvlNine++;
                        npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlNine;
                        npcDescription = "(9, 9, 9, 9, 9)";
                    }
                    npcs.Add(new Npc
                    {
                        Name = npcName,
                        Description = npcDescription,
                        Deck = validNpcDeckEntries,
                        Level = npcLvl,
                        IsDeleted = false,
                    });

                    //This while loop will stop when the list of NPCs has 10 NPCs
                }

                return npcs;
            }
            else if (level == 1)
            {
                var npcs = new List<Npc>();
                var countBotsLvlOne = 0;
                var count = 1;

                while (count <= 2)
                {
                    // Obtaining the lists of all unique sequences
                    // (for cardLvl 1 there are 5 unique possible combinations):
                    for (int i = 8; i <= 12; i++)
                    {
                        // Obtaining the list of all 5 unique sequence for cardLvl 1)
                        var levelSequence = Helper.GetPowerSequence(level, i);

                        // Criating a new list of valid NPC deck entries:
                        var validNpcDeckEntries = new List<NpcDeckEntry>();

                        // Obtaing a random card of cardLvl corresponding to its position in the sequence:
                        foreach (var cardLvl in levelSequence)
                        {
                            // Filtering all cards whose cardLvl is 1:
                            var cardsFiltered = cardsDB.Where(a => a.Level == cardLvl).ToList();

                            // Obtaining one random card out of the list of filtered cards
                            var card = cardsFiltered.OrderBy(a => random.Next()).Take(1).FirstOrDefault();

                            // "Converting" the random card into a new NPC DECK ENTRY and adding it to a new list of valid deck entries:                               
                            validNpcDeckEntries.Add
                            (
                                new NpcDeckEntry
                                {
                                    Card = card
                                }
                            );
                        }

                        // Creating a new npc with 5 cards npcLvl 1 and adding it to a list of new NPCs:
                        countBotsLvlOne++;
                        var npcLvl = Helper.GetNpcLevel(validNpcDeckEntries.Select(a => a.Card.Level).ToList());
                        var npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlOne;
                        npcs.Add(new Npc
                        {
                            Name = npcName,
                            Description = "( " + string.Join(", ", levelSequence) + " )",
                            Deck = validNpcDeckEntries,
                            Level = npcLvl,
                            IsDeleted = false,
                        });

                    }
                    //This while loop will stop when the count is 2.                       
                    count++;
                }

                return npcs;
            }
            else if (level == 8)
            {
                var npcs = new List<Npc>();
                var countBotsLvlEight = 0;
                var count = 1;


                while (count <= 4)
                {
                    // Obtaining the lists of all unique sequences
                    // (for cardLvl 8 there are 3 unique possible combinations):
                    for (int i = 5; i <= 12; i++)
                    {
                        if (i == 5 || i == 7 || i == 12)
                        {
                            // Obtaining the list of all 3 unique sequence for cardLvl 8)
                            var levelSequence = Helper.GetPowerSequence(level, i);

                            // Criating a new list of valid NPC deck entries:
                            var validNpcDeckEntries = new List<NpcDeckEntry>();

                            // Obtaing a random cards of cardLvl corresponding to its position in the sequence:
                            foreach (var cardLvl in levelSequence)
                            {
                                // Filtering all cards whose cardLvl is 8:
                                var cardsFiltered = cardsDB.Where(a => a.Level == cardLvl).ToList();

                                // Obtaining one random card out of the list of filtered cards
                                var card = cardsFiltered.OrderBy(a => random.Next()).FirstOrDefault();

                                // "Converting" the random card into a new NPC DECK ENTRY and adding it to a new list of valid deck entries:                               
                                validNpcDeckEntries.Add
                                (
                                    new NpcDeckEntry
                                    {
                                        Card = card
                                    }
                                );
                            }

                            // Creating a new npc with 5 cards npcLvl 1 and adding it to a list of new NPCs:                        
                            countBotsLvlEight++;
                            var npcLvl = Helper.GetNpcLevel(validNpcDeckEntries.Select(a => a.Card.Level).ToList());
                            var npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlEight;
                            npcs.Add(new Npc
                            {
                                Name = npcName,
                                Description = "( " + string.Join(", ", levelSequence) + " )",
                                Deck = validNpcDeckEntries,
                                Level = npcLvl,
                                IsDeleted = false,
                            });
                        }
                    }

                    count++;
                    //This while loop will stop when count is 4, since there 3 possible sequences looping 4 times will result in 12 npcs. 
                }

                //Returns only the first 10 npcs
                return npcs.Take(10).ToList();
            }
            else //(cardLvl != 0 && cardLvl != 1 && cardLvl != 8 && cardLvl != 9)
            {
                var countBotsLvlTwo = 0;
                var countBotsLvlThree = 0;
                var countBotsLvlFour = 0;
                var countBotsLvlFive = 0;
                var countBotsLvlSix = 0;
                var countBotsLvlSeven = 0;

                var validDecks = new List<List<NpcDeckEntry>>();
                var npcs = new List<Npc>();

                // Obtaining the lists of all unique sequences
                // (for cardLvl 2 up to 7 there are 12 unique possible combinations):
                for (int i = 1; i <= 12; i++)
                {
                    // Criating a new list of valid NPC deck entries:
                    var validNpcDeckEntries = new List<NpcDeckEntry>();

                    //ex.: cardLvl == 6 and  i == 1 => (4, 4, 6, 8, 8 )
                    var levelSequence = Helper.GetPowerSequence(level, i);

                    // Obtaing a random card of cardLvl corresponding to its position in the sequence:
                    foreach (var cardLvl in levelSequence)
                    {
                        // Filtering all cards by cardLvl (2, 3, 4, 5, 6 or 7):
                        var cardsFiltered = cardsDB.Where(a => a.Level == cardLvl).ToList();

                        // Obtaing a random card of cardLvl corresponding to its position in the sequence:
                        var card = cardsFiltered.OrderBy(a => random.Next()).FirstOrDefault();

                        // "Converting" the random card into a new NPC DECK ENTRY and adding it to a new list of valid deck entries:
                        validNpcDeckEntries.Add
                        (
                            new NpcDeckEntry
                            {
                                Card = card
                            }
                        );
                    }

                    // Creating a new npc with 5 cards and adding it to a list of new NPCs:   
                    var npcName = "";
                    var npcLvl = Helper.GetNpcLevel(validNpcDeckEntries.Select(a => a.Card.Level).ToList());

                    switch (level)
                    {
                        case 2:
                            countBotsLvlTwo++;
                            npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlTwo;
                            break;
                        case 3:
                            countBotsLvlThree++;
                            npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlThree;
                            break;
                        case 4:
                            countBotsLvlFour++;
                            npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlFour;
                            break;
                        case 5:
                            countBotsLvlFive++;
                            npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlFive;
                            break;
                        case 6:
                            countBotsLvlSix++;
                            npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlSix;
                            break;
                        case 7:
                            countBotsLvlSeven++;
                            npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlSeven;
                            break;
                    }

                    npcs.Add(new Npc
                    {
                        Name = npcName,
                        Description = "( " + string.Join(", ", levelSequence) + " )",
                        Deck = validNpcDeckEntries,
                        Level = npcLvl,
                        IsDeleted = false,
                    });

                    // By the end of this for loop, there will be 12 npcs                  
                }

                // Returns 10 random npcs out of the list of 12: 
                return npcs.OrderBy(a => random.Next()).Take(10).ToList(); ;
            }
        }

        public async Task<(List<AdminsGetNpcsResponse>, string)> GetNpcs(AdminsGetNpcsRequest request)
        {
            var contentQueriable = _daoDbContext
                              .Npcs
                              .AsNoTracking()
                              .Where(a => a.IsDeleted == false);

            var message = "All NPCs listed successfully";

            if (request.StartNpcId.HasValue && request.EndNpcId.HasValue == true)
            {
                contentQueriable = contentQueriable.Where(a => a.Id >= request.StartNpcId && a.Id <= request.EndNpcId);

                message = "NPCs listed successfully";

                if (request.StartNpcId == request.EndNpcId)
                {
                    message = "NPC listed successfully";
                }
            }

            var content = await contentQueriable
                .Select(a => new AdminsGetNpcsResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Deck = a.Deck
                        .Select(b => new AdminsGetNpcsResponse_Deck
                        {
                            Id = b.Id,
                            Name = b.Card.Name,
                            Power = b.Card.Power,
                            UpperHand = b.Card.UpperHand,
                            Level = b.Card.Level,
                            Type = b.Card.Type,
                        })
                        .ToList(),
                    Level = a.Level
                })
                .OrderBy(a => a.Level)
                .ThenBy(a => a.Name)
                .ToListAsync();

            return (content, message);
        }

        public async Task<(AdminsFlexEditNpcResponse?, string)> FlexEdit(AdminsFlexEditNpcRequest request)
        {
            var (isValid, message) = FlexUpdateIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var npcDB = await _daoDbContext
                                  .Npcs
                                  .Include(a => a.Deck)
                                  .ThenInclude(a => a.Card)
                                  .FirstOrDefaultAsync(a => a.Id == request.Id && a.IsDeleted == false);

            if (npcDB == null)
            {
                return (null, $"Error: NPC not found: {request.Name}");
            }

            if (string.IsNullOrEmpty(request.Name) == false)
            {
                npcDB.Name = request.Name;
            }

            if (string.IsNullOrEmpty(request.Description) == false)
            {
                npcDB.Description = request.Description;
            }

            if (request.DeckChanges != null && request.DeckChanges.Count != 0)
            {
                if (npcDB.Deck.Count == 0)
                {
                    return (null, "Error: NPC deck is empty");
                }

                var oldIds = npcDB.Deck
                                  .Select(a => a.Card.Id)
                                  .ToList();

                if (request.DeckChanges.Keys.All(id => oldIds.Contains(id)) == false)
                {
                    return (null, "Error: the card to be replaced was not found");
                }

                var availableCardIds = _daoDbContext
                                           .NpcDeckEntries
                                           .Select(a => a.CardId)
                                           .ToList();

                if (request.DeckChanges.Values.All(id => availableCardIds.Contains(id)) == false)
                {
                    return (null, "Error: the cardId provided leads to a non-existing card");
                }

                foreach (var oldId in oldIds)
                {
                    if (request.DeckChanges.ContainsKey(oldId))
                    {
                        _daoDbContext
                            .NpcDeckEntries
                            .Where(a => a.Npc.Id == request.Id && a.Card.Id == oldId)
                            .ExecuteUpdate(a => a.SetProperty(b => b.CardId, request.DeckChanges[oldId]));
                    }
                }
            }

            await _daoDbContext.SaveChangesAsync();

            return (null, "NPC updated successfully");
        }
        private static (bool, string) FlexUpdateIsValid(AdminsFlexEditNpcRequest request)
        {
            if (request == null)
            {
                return (false, "Error: no information was provided");
            }

            if (request.DeckChanges != null && request.DeckChanges.Count > 5)
            {
                return (false, "Error: the NPC's deck cannot contain more than 5 cards");
            }

            return (true, string.Empty);
        }

        public async Task<(AdminsEditNpcResponse?, string)> EditNpc(AdminsEditNpcRequest request)
        {
            var (isValid, message) = UpdateIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var npcDB = await _daoDbContext
                                  .Npcs
                                  .Include(a => a.Deck)
                                  .ThenInclude(b => b.Card)
                                  .FirstOrDefaultAsync(a => a.Id == request.Id && a.IsDeleted == false);

            if (npcDB == null)
            {
                return (null, $"Error: NPC not found: {request.Name}");
            }

            var availableCardIds = _daoDbContext
                                       .Cards
                                       .Where(a => a.IsDeleted == false)
                                       .Select(a => a.Id)
                                       .ToList();

            if (request.CardIds.All(id => availableCardIds.Contains(id)) == false)
            {
                return (null, "Error: the cardId or cardIds provided lead to non-existing cards");
            }

            var oldCardIds = npcDB.Deck
                                  .Select(a => a.Id)
                                  .ToList();

            await _daoDbContext.NpcDeckEntries
                      .Where(a => oldCardIds.Contains(a.CardId) && a.NpcId == request.Id)
                      .ExecuteDeleteAsync();

            var (newNpcDeckEntries, ErrorMessage) = await GenerateRandomDeck(request.CardIds);

            if (newNpcDeckEntries == null || newNpcDeckEntries.Count != 5)
            {
                return (null, ErrorMessage);
            }

            npcDB.Name = request.Name;
            npcDB.Description = request.Description;
            npcDB.Deck = newNpcDeckEntries;
            npcDB.Level = Helper.GetNpcLevel(newNpcDeckEntries.Select(a => a.Card.Level).ToList());

            await _daoDbContext.SaveChangesAsync();

            return (null, "NPC updated successfully");
        }
        private (bool, string) UpdateIsValid(AdminsEditNpcRequest request)
        {
            if (request == null)
            {
                return (false, "Error: no information was provided for creating a new NPC");
            }

            if (string.IsNullOrWhiteSpace(request.Name) == true)
            {
                return (false, "Error: the NPC's name is mandatory");
            }

            if (string.IsNullOrEmpty(request.Description) == true)
            {
                return (false, "Error: the NPC's description is mandatory");
            }

            if (request.CardIds == null || request.CardIds.Count != Constants.DeckSize)
            {
                return (false, $"Error: the NPC's deck can neither be empty nor contain fewer or more than {Constants.DeckSize} cards");
            }

            return (true, string.Empty);
        }

        public async Task<(AdminsDeleteNpcResponse?, string)> DeleteNpc(AdminsDeleteNpcRequest request)
        {
            if (request.NpcId <= 0)
            {
                return (null, $"Error: invalid NpcId");
            }


            var exists = await _daoDbContext
                                   .Npcs
                                   .AnyAsync(a => a.Id == request.NpcId && a.IsDeleted == false);

            if (exists == false)
            {
                return (null, $"Error: NpcId not found");
            }

            await _daoDbContext
                      .Npcs
                      .Where(a => a.Id == request.NpcId)
                      .ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

            return (null, "NPC deleted successfully");
        }
        private async Task<(List<NpcDeckEntry>?, string)> GenerateRandomDeck(List<int> cardIds)
        {
            var cardsDB = await _daoDbContext
                                    .Cards
                                    .Where(a => cardIds.Contains(a.Id) && a.IsDeleted == false)
                                    .ToListAsync();

            if (cardsDB == null || cardsDB.Count == 0)
            {
                return (null, "Error: invalid card Ids");
            }

            var uniqueCardIds = cardIds.Distinct()
                                       .ToList()
                                       .Count;

            if (uniqueCardIds != cardsDB.Count)
            {
                var notFoundIds = cardIds.Distinct()
                                         .ToList()
                                         .Except(cardsDB.Select(a => a.Id).ToList());

                return (null, $"Error: invalid cardId: {string.Join(" ,", notFoundIds)}");
            }

            var newDeck = new List<NpcDeckEntry>();

            foreach (var id in cardIds)
            {
                var newCard = cardsDB.FirstOrDefault(a => a.Id == id);
                if (newCard != null)
                {
                    newDeck.Add(new NpcDeckEntry()
                    {
                        Card = newCard,
                    });
                }
            }

            return (newDeck, string.Empty);
        }
        #endregion

        #region Admin DB Data deletion
        public async Task<(AdminsDeleteDbDataResponse?, string)> DeleteDbData(AdminsDeleteDbDataRequest response)
        {     
            await using var transaction = await _daoDbContext.Database.BeginTransactionAsync();

            var message = "DB data deletion successful";

            try
            {
                await this._daoDbContext.Cards.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

                await this._daoDbContext.Npcs.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

                await this._daoDbContext.Saves.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

                await this._daoDbContext.Decks.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

                await transaction.CommitAsync(); // ✅ Commit changes if everything is successful

                return (null, "DB data deletion successful");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // ❌ Rollback in case of any failure

                return (null, $"DB data deletion failed: {ex.Message}");
            }         
        }
        #endregion
    }
}
