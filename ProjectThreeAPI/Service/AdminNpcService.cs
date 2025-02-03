﻿using MedievalAutoBattler.Models.Dtos.Request;
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

            var newNpc = new Npc
            {
                Name = npc.Name,
                Description = npc.Description,
                Deck = new List<DeckEntry>(),
                IsDeleted = false
            };

            newNpc.Deck = await GetNewDeck(npc.Deck);

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

        public async Task<(List<AdminNpcReadResponse>, string)> Read()
        {
            return (await this._daoDbContext
                .Npcs
                .AsNoTracking()
                .Where(a => a.IsDeleted == false)
                .Select(a => new AdminNpcReadResponse
                {
                    Id = a.Id,
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
                .ToListAsync(), "Read Successful");
        }

        public async Task<string> Update(AdminNpcUpdateRequest npc)
        {
            var (isValid, message) = this.UpdateIsValid(npc);
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

                var availableCardIds = _daoDbContext.DeckEntries.Select(a => a.CardId).ToList();
                if (npc.DeckChanges.Values.All(id => availableCardIds.Contains(id))
                    == false)
                {
                    return "Error: the cardId provided leads to a non-existing card";
                }

                foreach (var oldId in oldIds)
                {
                    if (npc.DeckChanges.ContainsKey(oldId))
                    {
                        this._daoDbContext.DeckEntries
                                           .Where(a => a.Npc.Id == npc.Id && a.Card.Id == oldId)
                                           .ExecuteUpdate(b => b.SetProperty(a => a.CardId, npc.DeckChanges[oldId]));
                    }
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

            if (npc.DeckChanges != null && npc.DeckChanges.Count > 5)
            {
                return (false, "The NPC's hand cannot contain more than 5 cards");
            }

            return (true, String.Empty);
        }

        private async Task<List<DeckEntry>> GetNewDeck(List<int> cardsId)
        {
            var cardsDB = await this._daoDbContext
              .Cards
              .Where(a => cardsId.Contains(a.Id))
              .ToListAsync();

            foreach (var id in cardsId)
            {
                if (cardsDB.Select(a => a.Id).Contains(id) == false)
                {

                    return [];
                }
            }

            var newDeck = new List<DeckEntry>();

            foreach (var id in cardsId)
            {
                var newCard = cardsDB.Where(a => a.Id == id).FirstOrDefault();
                if (newCard != null)
                {
                    newDeck.Add(new DeckEntry()
                    {
                        Card = newCard,
                        IsDeleted = false
                    });
                }
            }

            return newDeck;
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
