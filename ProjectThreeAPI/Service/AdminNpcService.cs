using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.OpenApi.Any;
using ProjectThreeAPI.Models.Dtos.Request;
using ProjectThreeAPI.Models.Dtos.Response;
using ProjectThreeAPI.Models.Entities;
using ProjectThreeAPI.Utilities;
using System.Reflection.Emit;
using static System.Net.WebRequestMethods;

namespace ProjectThreeAPI.Service
{
    public class AdminNpcService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public AdminNpcService(ApplicationDbContext daoDBcontext)
        {
            this._daoDbContext = daoDBcontext;
        }

        public async Task<string> Create(AdminNpcCreateRequest npc)
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

            var cardsDB = await this._daoDbContext
              .Cards
              .Where(a => npc.Deck.Contains(a.Id))
              .ToListAsync();

            foreach (var id in npc.Deck)
            {
                if (cardsDB.Select(a => a.Id).Contains(id) == false)
                {
                    return $"Error: card Id not found: {id}";
                }
            }

            var newNpc = new Npc
            {
                Name = npc.Name,
                Description = npc.Description,
                Deck = new List<DeckEntry>(),
                IsDeleted = false
            };                 

            foreach (var id in npc.Deck)
            {
                var newCard = cardsDB.Where(a => a.Id == id).FirstOrDefault();
                if (newCard != null)
                {
                    newNpc.Deck.Add(new DeckEntry()
                    {
                        Card = newCard
                    });
                }
            }

            newNpc.Level = Helper.GetNpcLevel(newNpc);

            _daoDbContext.Add(newNpc);

            await _daoDbContext.SaveChangesAsync();

            return "Create action successful";
        }
        public (bool, string) CreateIsValid(AdminNpcCreateRequest npc)
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

            if (npc.Deck.Count == null || npc.Deck.Count != 5)
            {
                return (false, "The NPC's hand can neither be empty nor contain fewer or more than 5 cards");
            }

            return (true, String.Empty);
        }

        public async Task<List<AdminNpcReadResponse>> Read()
        {
            return await this._daoDbContext
                .Npcs
                .AsNoTracking()
                .Where(a => a.IsDeleted == false)
                .Select(a => new AdminNpcReadResponse
                {
                    Name = a.Name,
                    Description = a.Description,
                    Hand = a.Deck
                            .Select(b =>
                            new HandOfCardsResponse
                            {
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
        }

        public async Task<string> Update(AdminNpcUpdateRequest npc)
        {
            var (isValid, message) = this.UpdateIsValid(npc);

            if (isValid == false)
            {
                return message;
            }

            var npcDb = await _daoDbContext
                .Npcs
                .Where(a => a.Id == npc.Id && a.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (npcDb == null)
            {
                return $"Npc not found: {npc.Name}";
            }

            var cardsDB = await this._daoDbContext
              .Cards
              .Where(a => npc.Deck.Contains(a.Id))
              .ToListAsync();

            foreach (var id in npc.Deck)
            {
                if (cardsDB.Select(a => a.Id).Contains(id) == false)
                {
                    return $"Error: card Id not found: {id}";
                }
            }

            npcDb.Name = npc.Name;
            npcDb.Description = npc.Description;
            npcDb.Deck = new List<DeckEntry>();

            foreach (var id in npc.Deck)
            {
                var newCard = cardsDB.Where(a => a.Id == id).FirstOrDefault();
                if (newCard != null)
                {
                    npcDb.Deck.Add(new DeckEntry()
                    {
                        Card = newCard
                    });
                }
            }

            await _daoDbContext.SaveChangesAsync();

            return "Update action successful";
        }

        private (bool, string) UpdateIsValid(AdminNpcUpdateRequest npc)
        {
            if (npc == null)
            {
                return (false, "No information provided");
            }

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

            if (npc.Deck.Count == null || npc.Deck.Count != 5)
            {
                return (false, "The NPC's hand can neither be empty nor contain fewer or more than 5 cards");
            }

            return (true, String.Empty);
        }

        public async Task<string> Delete(int id)
        {
            if (id == null || id <= 0)
            {
                return $"Error: Invalid Npc ID, ID cannot be empty or equal/lesser than 0";
            }

            var exists = await this._daoDbContext
                .Npcs
                .Where(a => a.Id == id && a.IsDeleted == false)
                .AnyAsync();

            if (exists == false)
            {
                return $"Error: Invalid Npc ID, the npc does not exist or is already deleted";
            }

            await this._daoDbContext
               .Npcs
               .Where(a => a.Id == id)
               .ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

            return "Delete action successful";
        }
    }
}
