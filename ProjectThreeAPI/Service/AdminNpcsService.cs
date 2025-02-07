using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Models.Entities;
using Microsoft.EntityFrameworkCore;
using MedievalAutoBattler.Utilities;
using MedievalAutoBattler.Models.Enums;
using System;
using System.Reflection.Emit;

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

            var (newNpcDeckEntries, ErrorMessage) = await this.GetNewDeck(request.CardIds);

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

            return (null, "Create successful");
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

            if (request.CardIds.Count == 0 || request.CardIds.Count != 5)
            {
                return (false, "Error: the NPC's deck can neither be empty nor contain fewer or more than 5 cards");
            }

            return (true, String.Empty);
        }


        public async Task<(AdminNpcsCreateResponse?, string)> Populate(AdminNpcsCreateRequest_populate request)
        {
            var cardsDB = await this._daoDbContext.Cards.ToListAsync();
            var npcsPopulation = new List<Npc>();
            var random = new Random();

            var count = 1;
            var botCount = 0;
            var step = 1;

            while (count <= 10)
            {
                var validCardsLvlZero = cardsDB.Where(c => c.Level == 0).ToList(); // Select as cards that could form a deck lvl 1 
                var deckCards = validCardsLvlZero.OrderBy(_ => random.Next()).Take(5).ToList(); // Select 5 random cards

                var noobDeckEntry = new List<NpcDeckEntry>();
                foreach (var card in deckCards)
                {
                    noobDeckEntry.Add(new NpcDeckEntry
                    {
                        Card = card,
                    });
                }

                botCount++;
                var npcLvlZero = new Npc
                {
                    Name = "BOT " + botCount,
                    Description = "LvL*0*" + "|" + "*" + step + "*",
                    Deck = noobDeckEntry,
                    Level = Helper.GetNpcLevel(noobDeckEntry.Select(a => a.Card.Level).ToList()),
                    IsDeleted = false
                };
                npcsPopulation.Add(npcLvlZero);
                count++;
                step++;
            }

            step = 1;

            this._daoDbContext.AddRange(npcsPopulation);

            await this._daoDbContext.SaveChangesAsync();

            return (null, "Create successful");
        }



        public async Task<(List<AdminNpcsReadResponse>, string)> Read()
        {
            var content = await this._daoDbContext
                              .Npcs
                              .AsNoTracking()
                              .Where(a => a.IsDeleted == false)
                              .Select(a => new AdminNpcsReadResponse
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Description = a.Description,
                                  Deck = a.Deck
                                          .Select(b => new AdminNpcReadResponse_Deck
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

            return (content, "Read Successful");
        }

        public async Task<(AdminNpcsFlexUpdateResponse?, string)> FlexUpdate(AdminNpcsFlexUpdateRequest request)
        {
            var (isValid, message) = this.FlexUpdateIsValid(request);

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

            return (null, "Update action successful");
        }
        private (bool, string) FlexUpdateIsValid(AdminNpcsFlexUpdateRequest request)
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

            var (newNpcDeckEntries, ErrorMessage) = await this.GetNewDeck(request.CardIds);

            if (newNpcDeckEntries == null || newNpcDeckEntries.Count != 5)
            {
                return (null, ErrorMessage);
            }

            npcDB.Name = request.Name;
            npcDB.Description = request.Description;
            npcDB.Deck = newNpcDeckEntries;
            npcDB.Level = Helper.GetNpcLevel(newNpcDeckEntries.Select(a => a.Card.Level).ToList());

            await this._daoDbContext.SaveChangesAsync();

            return (null, "Update action successful");
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

            if (request.CardIds == null || request.CardIds.Count == 0 || request.CardIds.Count != 5)
            {
                return (false, "Error: the NPC's deck can neither be empty nor contain fewer or more than 5 cards");
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

            return (null, "Delete successful");
        }
        private async Task<(List<NpcDeckEntry>?, string)> GetNewDeck(List<int> cardIds)
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
