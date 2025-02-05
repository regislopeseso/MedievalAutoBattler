using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.OpenApi.Any;
using ProjectThreeAPI.Models.Dtos.Request;
using ProjectThreeAPI.Models.Dtos.Response;
using ProjectThreeAPI.Models.Entities;
using ProjectThreeAPI.Utilities;
using System.Linq;
using System.Reflection.Emit;
using static System.Net.WebRequestMethods;

namespace ProjectThreeAPI.Service
{
    public class AdminNpcsService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public AdminNpcsService(ApplicationDbContext daoDBcontext)
        {
            this._daoDbContext = daoDBcontext;
        }

        public async Task<string> Create(AdminNpcsCreateRequest npc)
        {
            var (isValid, message) = this.CreateIsValid(npc);
            if (isValid == false)
            {
                return message;
            }

            var exists = await this._daoDbContext
                .Npcs
                .Where(a => a.Name == npc.Name && a.IsDeleted == false)
                .AnyAsync();
            if (exists == true)
            {
                return $"Error: this NPC already exists - {npc.Name}";
            }            

            var (newNpcDeckEntries, ErrorMessage) = await this.GetNewDeck(npc.CardIds);
            if (newNpcDeckEntries == null || newNpcDeckEntries.Count != 5)
            {
                return ErrorMessage;
            }            

            var newNpc = new Npc
            {
                Name = npc.Name,
                Description = npc.Description,
                Deck = newNpcDeckEntries,
                Level = Helper.GetNpcLevel(newNpcDeckEntries.Select(a => a.Card.Level).ToList()),
                IsDeleted = false
            };          
       
            _daoDbContext.Add(newNpc);

            await _daoDbContext.SaveChangesAsync();

            return "Create action successful";
        }
        public (bool, string) CreateIsValid(AdminNpcsCreateRequest npc)
        {
            if (npc == null)
            {
                return (false, "The information was provided");
            }

            if (string.IsNullOrEmpty(npc.Name) == true)
            {
                return (false, "An NPC's name is mandatory");
            }

            if (string.IsNullOrEmpty(npc.Description) == true)
            {
                return (false, "The NPC's description is mandatory");
            }

            if (npc.CardIds.Count == 0 || npc.CardIds.Count != 5)
            {
                return (false, "The NPC's deck can neither be empty nor contain fewer or more than 5 cards");
            }

            return (true, String.Empty);
        }

        public async Task<(List<AdminNpcsReadResponse>, string)> Read()
        {
            return (await this._daoDbContext
                .Npcs
                .AsNoTracking()
                .Where(a => a.IsDeleted == false)
                .Select(a => new AdminNpcsReadResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Deck = a.Deck
                            .Select(b =>
                            new AdminNpcReadResponse_Deck
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
                .ToListAsync(), "Read Successful");
        }

        public async Task<string> FlexUpdate(AdminNpcsFlexUpdateRequest npc)
        {
            var (isValid, message) = this.FlexUpdateIsValid(npc);
            if (isValid == false)
            {
                return message;
            }

            var npcDB = await _daoDbContext
                .Npcs
                .Include(a => a.Deck)
                .ThenInclude(a => a.Card)
                .Where(a => a.Id == npc.Id && a.IsDeleted == false)
                .FirstOrDefaultAsync();
            if (npcDB == null)
            {
                return $"Npc not found: {npc.Name}";
            }

            if (string.IsNullOrEmpty(npc.Name) == false)
            {
                npcDB.Name = npc.Name;
            }

            if (string.IsNullOrEmpty(npc.Description) == false)
            {
                npcDB.Description = npc.Description;
            }



            if (npc.DeckChanges != null && npc.DeckChanges.Count != 0)
            {
                if (npcDB.Deck.Count == 0)
                {
                    return "Error: NPC deck is empty.";
                }

                var oldIds = npcDB.Deck.Select(a => a.Card.Id).ToList();
                if (npc.DeckChanges.Keys.All(id => oldIds.Contains(id))
                    == false)
                {
                    return "Error: the card to be replaced was not found";
                }

                var availableCardIds = _daoDbContext.NpcDeckEntries.Select(a => a.CardId).ToList();
                if (npc.DeckChanges.Values.All(id => availableCardIds.Contains(id))
                    == false)
                {
                    return "Error: the cardId provided leads to a non-existing card";
                }

                foreach (var oldId in oldIds)
                {
                    if (npc.DeckChanges.ContainsKey(oldId))
                    {
                        this._daoDbContext.NpcDeckEntries
                                           .Where(a => a.Npc.Id == npc.Id && a.Card.Id == oldId)
                                           .ExecuteUpdate(b => b.SetProperty(a => a.CardId, npc.DeckChanges[oldId]));
                    }
                }
            }

            await _daoDbContext.SaveChangesAsync();

            return "Update action successful";
        }
        private (bool, string) FlexUpdateIsValid(AdminNpcsFlexUpdateRequest npc)
        {
            if (npc == null)
            {
                return (false, "No information provided");
            }

            if (npc.DeckChanges != null && npc.DeckChanges.Count > 5)
            {
                return (false, "The NPC's deck cannot contain more than 5 cards");
            }

            return (true, String.Empty);
        }

        public async Task<string> Update(AdminNpcsUpdateRequest npc)
        {
            var (isValid, message) = this.UpdateIsValid(npc);
            if (isValid == false)
            {
                return message;
            }

            var npcDB = await _daoDbContext
                .Npcs
                .Include(a => a.Deck)
                .ThenInclude(b => b.Card)
                .Where(a => a.Id == npc.Id && a.IsDeleted == false)
                .FirstOrDefaultAsync();            

            if (npcDB == null)
            {
                return $"Npc not found: {npc.Name}";
            }

            var availableCardIds = _daoDbContext.Cards.Where(a => a.IsDeleted == false).Select(a => a.Id).ToList();
            if (npc.CardIds.All(id => availableCardIds.Contains(id))
                == false)
            {
                return "Error: the cardId or cardIds provided lead to non-existing cards";
            }

            var oldCardIds = npcDB.Deck.Select(a => a.Id).ToList();
            await this._daoDbContext.NpcDeckEntries
                              .Where(a => oldCardIds.Contains(a.CardId) && a.NpcId == npc.Id)
                              .ExecuteDeleteAsync();

            var (newNpcDeckEntries, ErrorMessage) = await this.GetNewDeck(npc.CardIds);
            if (newNpcDeckEntries == null || newNpcDeckEntries.Count != 5)
            {
                return ErrorMessage;
            }

            npcDB.Name = npc.Name;
            npcDB.Description = npc.Description;
            npcDB.Deck = newNpcDeckEntries;
            npcDB.Level = Helper.GetNpcLevel(newNpcDeckEntries.Select(a => a.Card.Level).ToList());

            await _daoDbContext.SaveChangesAsync();

            return "Update action successful";
        }
        private (bool, string) UpdateIsValid(AdminNpcsUpdateRequest npc)
        {
            if (npc == null)
            {
                return (false, "Error: no information was provided for creating a new NPC");
            }

            if (string.IsNullOrEmpty(npc.Name) == true)
            {
                return (false, "Error: the NPC's name is mandatory");
            }

            if (string.IsNullOrEmpty(npc.Description) == true)
            {
                return (false, "Error: the NPC's description is mandatory");
            }

            if (npc.CardIds == null || npc.CardIds.Count == 0 || npc.CardIds.Count != 5)
            {
                return (false, "Error: the NPC's deck can neither be empty nor contain fewer or more than 5 cards");
            }

            return (true, String.Empty);
        }


        public async Task<string> Delete(int npcId)
        {
            if (npcId <= 0)
            {
                return $"Error: Invalid Npc ID, ID cannot be empty or equal/lesser than 0";
            }

            var exists = await this._daoDbContext
                .Npcs
                .AnyAsync(a => a.Id == npcId && a.IsDeleted == false);

            if (exists == false)
            {
                return $"Error: Invalid Npc ID, the npc does not exist or is already deleted";
            }

            await this._daoDbContext
               .Npcs
               .Where(a => a.Id == npcId)
               .ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

            return "Delete action successful";
        }
        private async Task<(List<NpcDeckEntry>, string)> GetNewDeck(List<int> cardIds)
        {
            var cardsDB = await this._daoDbContext
              .Cards
              .Where(a => cardIds.Contains(a.Id) && a.IsDeleted == false)
              .ToListAsync();
            if (cardsDB == null || cardsDB.Count == 0)
            {
                return ([], "Error: invalid card Ids");
            }

            var uniqueCardIds = cardIds.Distinct().ToList().Count;
            if (uniqueCardIds != cardsDB.Count)
            {
                var notFoundIds = cardIds.Distinct().ToList().Except(cardsDB.Select(a => a.Id).ToList());
                return ([], $"Error: invalid cardId: {string.Join(" ,", notFoundIds)}");
            }
          
            var newDeck = new List<NpcDeckEntry>();

            foreach (var id in cardIds)
            {
                var newCard = cardsDB.Where(a => a.Id == id).FirstOrDefault();
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
