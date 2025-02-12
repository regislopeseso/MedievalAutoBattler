using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Models.Entities;
using Microsoft.EntityFrameworkCore;
using MedievalAutoBattler.Utilities;
using MedievalAutoBattler.Models.Enums;
using System;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using System.Net.Sockets;
using System.Xml;

namespace MedievalAutoBattler.Service
{
    public class AdminNpcsService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public AdminNpcsService(ApplicationDbContext daoDBcontext)
        {
            this._daoDbContext = daoDBcontext;
        }

        public async Task<(AdminNpcsCreateResponse?, string)> Create(AdminNpcsCreateRequest request)
        {
            var (isValid, message) = this.CreateIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var exists = await this._daoDbContext
                                   .Npcs
                                   .AnyAsync(a => a.Name == request.Name && a.IsDeleted == false);

            if (exists == true)
            {
                return (null, $"Error: this NPC already exists - {request.Name}");
            }

            var (newNpcDeckEntries, ErrorMessage) = await this.GenerateRandomDeck(request.CardIds);

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

            this._daoDbContext.Add(newNpc);

            await this._daoDbContext.SaveChangesAsync();

            return (null, "NPC created successfully");
        }
        public (bool, string) CreateIsValid(AdminNpcsCreateRequest request)
        {
            if (request == null)
            {
                return (false, "Error: no information was provided");
            }

            if (string.IsNullOrEmpty(request.Name) == true)
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

            return (true, String.Empty);
        }

        public async Task<(AdminNpcsCreateResponse?, string)> Seed(AdminNpcsCreateRequest_seed request)
        {
            var cardsDB = await this._daoDbContext
                                    .Cards
                                    .Where(a => a.IsDeleted == false)
                                    .ToListAsync();

            if (cardsDB == null || cardsDB.Count == 0)
            {
                return (null, "Error: cards not found");
            }

            //Se não existir pelo menos uma carta de cada level não é possível fazer o seed dos NPCs.
            var countCardsLvl = cardsDB.GroupBy(a => a.Level).Count();
            if(countCardsLvl < Constants.MaxCardLvl - Constants.MinCardLvl)
            {
                return (null, "Error: not enough card variety for seeding NPCs. The existance of at least one card of each level is mandatory for seeding NPCs");
            }

            var npcsSeed = new List<Npc>();

            for (int level = Constants.MinCardLvl; level <= Constants.MaxCardLvl; level++)
            {
                npcsSeed.AddRange(GenerateRandomNpcs(level, cardsDB));
            }

            if(npcsSeed == null || npcsSeed.Count == 0)
            {
                return (null, "Error: seeding NPCs failed");
            }

            this._daoDbContext.AddRange(npcsSeed);

            await this._daoDbContext.SaveChangesAsync();

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

        public async Task<(List<AdminNpcsReadResponse>, string)> GetNpcs(AdminNpcsGetRequest request)
        {
            var contentQueriable = this._daoDbContext
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
                .Select(a => new AdminNpcsReadResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Deck = a.Deck
                        .Select(b => new AdminNpcGetResponse_Deck
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

        public async Task<(AdminNpcsFlexUpdateResponse?, string)> FlexUpdate(AdminNpcsFlexUpdateRequest request)
        {
            var (isValid, message) = FlexUpdateIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var npcDB = await this._daoDbContext
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

                var availableCardIds = this._daoDbContext
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
                        this._daoDbContext
                            .NpcDeckEntries
                            .Where(a => a.Npc.Id == request.Id && a.Card.Id == oldId)
                            .ExecuteUpdate(a => a.SetProperty(b => b.CardId, request.DeckChanges[oldId]));
                    }
                }
            }

            await this._daoDbContext.SaveChangesAsync();

            return (null, "NPC updated successfully");
        }
        private static (bool, string) FlexUpdateIsValid(AdminNpcsFlexUpdateRequest request)
        {
            if (request == null)
            {
                return (false, "Error: no information was provided");
            }

            if (request.DeckChanges != null && request.DeckChanges.Count > 5)
            {
                return (false, "Error: the NPC's deck cannot contain more than 5 cards");
            }

            return (true, String.Empty);
        }

        public async Task<(AdminNpcsUpdateResponse?, string)> Update(AdminNpcsUpdateRequest request)
        {
            var (isValid, message) = this.UpdateIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var npcDB = await this._daoDbContext
                                  .Npcs
                                  .Include(a => a.Deck)
                                  .ThenInclude(b => b.Card)
                                  .FirstOrDefaultAsync(a => a.Id == request.Id && a.IsDeleted == false);

            if (npcDB == null)
            {
                return (null, $"Error: NPC not found: {request.Name}");
            }

            var availableCardIds = this._daoDbContext
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

            await this._daoDbContext.NpcDeckEntries
                      .Where(a => oldCardIds.Contains(a.CardId) && a.NpcId == request.Id)
                      .ExecuteDeleteAsync();

            var (newNpcDeckEntries, ErrorMessage) = await this.GenerateRandomDeck(request.CardIds);

            if (newNpcDeckEntries == null || newNpcDeckEntries.Count != 5)
            {
                return (null, ErrorMessage);
            }

            npcDB.Name = request.Name;
            npcDB.Description = request.Description;
            npcDB.Deck = newNpcDeckEntries;
            npcDB.Level = Helper.GetNpcLevel(newNpcDeckEntries.Select(a => a.Card.Level).ToList());

            await this._daoDbContext.SaveChangesAsync();

            return (null, "NPC updated successfully");
        }
        private (bool, string) UpdateIsValid(AdminNpcsUpdateRequest request)
        {
            if (request == null)
            {
                return (false, "Error: no information was provided for creating a new NPC");
            }

            if (string.IsNullOrEmpty(request.Name) == true)
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

            return (true, String.Empty);
        }

        public async Task<(AdminNpcsDeleteResponse?, string)> Delete(AdminNpcsDeleteRequest request)
        {
            if (request.NpcId <= 0)
            {
                return (null, $"Error: invalid NpcId");
            }


            var exists = await this._daoDbContext
                                   .Npcs
                                   .AnyAsync(a => a.Id == request.NpcId && a.IsDeleted == false);

            if (exists == false)
            {
                return (null, $"Error: NpcId not found");
            }

            await this._daoDbContext
                      .Npcs
                      .Where(a => a.Id == request.NpcId)
                      .ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

            return (null, "NPC deleted successfully");
        }
        private async Task<(List<NpcDeckEntry>?, string)> GenerateRandomDeck(List<int> cardIds)
        {
            var cardsDB = await this._daoDbContext
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

            return (newDeck, String.Empty);
        }
    }
}
