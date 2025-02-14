using MedievalAutoBattler.Models.Dtos.Request.Players;
using MedievalAutoBattler.Models.Dtos.Response.Players;
using MedievalAutoBattler.Models.Entities;
using MedievalAutoBattler.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MedievalAutoBattler.Service
{
    public class PlayersService
    {
        public readonly ApplicationDbContext _daoDbContext;

        public PlayersService(ApplicationDbContext daoDbContext)
        {
            _daoDbContext = daoDbContext;
        }

        #region Players Saves
        public async Task<(PlayersNewSaveResponse?, string)> NewPlayer(PlayersNewSaveRequest request)
        {
            var (isValid, message) = NewPlayerIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var newSave = new Save
            {
                Name = request.Name,
            };

            var initialSaveCardEntries = new List<SaveCardEntry>();
            (initialSaveCardEntries, message) = await GetInitialSaveCardEntries(newSave.Id);
            if (initialSaveCardEntries == null || initialSaveCardEntries.Count == 0)
            {
                return (null, message);
            }

            var initialSaveDeckEntries = new List<SaveDeckEntry>();
            (initialSaveDeckEntries, message) = GetInitialSaveDeckEntries(initialSaveCardEntries);
            if (initialSaveDeckEntries == null || initialSaveDeckEntries.Count == 0)
            {
                return (null, message);
            }

            newSave.SaveCardEntries = initialSaveCardEntries;

            newSave.Decks = new List<Deck>()
            {
                new Deck
                {
                    Name = "Initial Deck",
                    SaveDeckEntries = initialSaveDeckEntries
                }
            };



            _daoDbContext.Add(newSave);

            await _daoDbContext.SaveChangesAsync();

            var content = new PlayersNewSaveResponse
            {
                SaveId = newSave.Id,
            };

            return (content, "A new save has been created successfully");
        }

        private (bool, string) NewPlayerIsValid(PlayersNewSaveRequest request)
        {
            if (request == null)
            {
                return (false, "Error: no information was provided");
            }

            if (string.IsNullOrWhiteSpace(request.Name) == true)
            {
                return (false, "Error: a name must be provided");
            }

            return (true, string.Empty);
        }
        private async Task<(List<SaveCardEntry>?, string)> GetInitialSaveCardEntries(int saveId)
        {
            var validInitialCardsDB = await _daoDbContext
                                    .Cards
                                    .Where(a => a.Power + a.UpperHand < 5 && a.IsDeleted == false)
                                    .Select(a => a.Id)
                                    .ToListAsync();

            if (validInitialCardsDB == null || validInitialCardsDB.Count == 0)
            {
                return (null, "Error: no cards having power + upper hand < 5 were found");
            }

            var random = new Random();
            var randomCardIds = new List<int>();

            while (randomCardIds.Count < Constants.DeckSize)
            {
                randomCardIds.Add(validInitialCardsDB[random.Next(validInitialCardsDB.Count)]);
            }

            var initialSaveCardEntries = new List<SaveCardEntry>();

            foreach (var cardId in randomCardIds)
            {
                if (cardId == 0)
                {
                    return (null, "Error: invalid cardId for initial SaveCardEntries");
                }

                initialSaveCardEntries.Add(new SaveCardEntry()
                {
                    SaveId = saveId,
                    CardId = cardId,
                });
            }

            return (initialSaveCardEntries, string.Empty);
        }
        private (List<SaveDeckEntry>?, string) GetInitialSaveDeckEntries(List<SaveCardEntry> initialSaveCardEntries)
        {
            var initialSaveDeckEntries = new List<SaveDeckEntry>();

            foreach (var initialSaveDeckEntry in initialSaveCardEntries)
            {
                initialSaveDeckEntries.Add(new SaveDeckEntry()
                {
                    SaveCardEntry = initialSaveDeckEntry
                });
            }

            if (initialSaveDeckEntries.Count == 0)
            {
                return (null, "Error: invalid cardId for initial deck");
            }

            return (initialSaveDeckEntries, string.Empty);
        }
        #endregion

        #region Players Cards
        public async Task<(List<PlayersGetCardsResponse>?, string)> GetCards(PlayersGetCardsRequest request)
        {
            if (request.SaveId <= 0)
            {
                return (null, "Error: invalid SaveId");
            }

            var collection = await _daoDbContext
                                       .SaveCardEntries
                                       .Where(a => a.Save.Id == request.SaveId)
                                       .Select(a => a.Card)
                                       .ToListAsync();

            if (collection == null || collection.Count == 0)
            {
                return (null, "Error: no cards found for this save.");
            }

            var content = collection
                .GroupBy(card => card.Id)
                .Select(group => new PlayersGetCardsResponse
                {
                    Card = group.First(),
                    Count = group.Count()
                })
                .OrderByDescending(a => a.Card.Level)
                .ToList();

            return (content, "Player's card collection read successfully");
        }
        #endregion

        #region Players Deck
        public async Task<(PlayersNewDeckResponse?, string)> NewDeck(PlayersNewDeckRequest request)
        {
            var (isValid, message) = NewDeckIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var saveDB = await _daoDbContext
                                   .Saves
                                   .Include(b => b.SaveCardEntries)
                                   .Include(a => a.Decks)
                                   .FirstOrDefaultAsync(a => a.Id == request.SaveId);

            if (saveDB == null)
            {
                return (null, "Error: invalid SaveId");
            }

            #region Checking if requested CardIds are available in players collection
            var playerSaveCardEntriesDB = saveDB.SaveCardEntries
                                                .GroupBy(a => a.CardId)
                                                .ToDictionary(cardId => cardId.Key, qty => qty.Count());

            // Group the requested cards to count their occurrences
            var requestedCardCounts = request.CardIds
                                             .GroupBy(cardId => cardId)
                                             .ToDictionary(cardId => cardId.Key, qty => qty.Count());

            // Check if the player has enough of each requested card
            foreach (var (cardId, requestedCount) in requestedCardCounts)
            {
                var cardExists = playerSaveCardEntriesDB.TryGetValue(cardId, out int ownedCount);

                if (cardExists == false)
                {
                    return (null, $"Error:  invalid cardIds {cardId}");
                }

                if (requestedCount > ownedCount)
                {
                    return (null, $"Error: Not enough copies of CardId {cardId}. Requested: {requestedCount}, Owned: {ownedCount}");
                }
            }
            #endregion

            var playerCardIdsDB = saveDB.SaveCardEntries.Select(a => a.CardId).ToList();


            var filteredSaveCardEntries = saveDB.SaveCardEntries
                                                .Where(entry => request.CardIds.Contains(entry.CardId))
                                                .ToList();
            var newDeck = new Deck()
            {
                Name = request.Name
            };

            var newSaveDeckEntries = new List<SaveDeckEntry>();
            foreach (var saveCardEntry in filteredSaveCardEntries)
            {
                newSaveDeckEntries.Add(
                    new SaveDeckEntry
                    {
                        SaveCardEntry = saveCardEntry,
                    });
            }
            newDeck.SaveDeckEntries = newSaveDeckEntries;

            saveDB.Decks.Add(newDeck);

            await _daoDbContext.SaveChangesAsync();

            return (null, "A new deck has been created successfully");
        }

        private (bool, string) NewDeckIsValid(PlayersNewDeckRequest request)
        {
            if (request == null)
            {
                return (false, "Error: no information was provided for creating a new Deck");
            }

            if (string.IsNullOrWhiteSpace(request.Name) == true)
            {
                return (false, "Error: the deck's name is mandatory");
            }

            if (request.SaveId <= 0)
            {
                return (false, "Error: SaveId invalid");
            }

            if (request.CardIds == null || request.CardIds.Count == 0 || request.CardIds.Count != 5)
            {
                return (false, "Error: the NPC's deck can neither be empty nor contain fewer or more than 5 cards");
            }

            return (true, string.Empty);
        }

        public async Task<(PlayersEditDeckResponse?, string)> EditDeck(PlayersEditDeckRequest request)
        {
            var (isValid, message) = EditDeckIsValid(request);

            if (isValid == false)
            {
                return (null, message);
            }

            var saveDB = await _daoDbContext
                                   .Saves
                                   .Include(a => a.Decks)
                                   .ThenInclude(a => a.SaveDeckEntries)
                                   .Include(a => a.SaveCardEntries)
                                   .FirstOrDefaultAsync(a => a.Decks.Any(deck => deck.Id == request.DeckId));

            if (saveDB == null)
            {
                return (null, $"Error: invalid deckId");
            }

            var deckDB = saveDB.Decks.FirstOrDefault(a => a.Id == request.DeckId);

            if (deckDB == null)
            {
                return (null, "Error: deck not found. Invalid DeckId");
            }

            if (deckDB.IsDeleted == true)
            {
                return (null, "Error: this deck was deleted");
            }

            var oldSaveCardEntriesIds = deckDB.SaveDeckEntries
                                              .Select(a => a.SaveCardEntry.Id)
                                              .ToList();

            #region Checking if requested CardIds are available in players collection
            var playerSaveCardEntriesDB = saveDB.SaveCardEntries
                                                .GroupBy(a => a.CardId)
                                                .ToDictionary(cardId => cardId.Key, qty => qty.Count());

            // Group the requested cards to count their occurrences
            var requestedCardCounts = request.CardIds
                                             .GroupBy(cardId => cardId)
                                             .ToDictionary(cardId => cardId.Key, qty => qty.Count());

            // Check if the player has enough of each requested card
            foreach (var (cardId, requestedCount) in requestedCardCounts)
            {
                var cardExists = playerSaveCardEntriesDB.TryGetValue(cardId, out int ownedCount);

                if (cardExists == false)
                {
                    return (null, $"Error:  invalid cardIds {cardId}");
                }

                if (requestedCount > ownedCount)
                {
                    return (null, $"Error: Not enough copies of CardId {cardId}. Requested: {requestedCount}, Owned: {ownedCount}");
                }
            }
            #endregion       

            var filteredSaveCardEntries = saveDB.SaveCardEntries
                                               .Where(entry => request.CardIds.Contains(entry.CardId))
                                               .ToList();

            deckDB.Name = request.Name;

            var updatedSaveDeckEntries = new List<SaveDeckEntry>();

            for (int i = 0; i < Constants.DeckSize; i++)
            {
                deckDB.SaveDeckEntries[i].SaveCardEntry = filteredSaveCardEntries[i];
            }

            await _daoDbContext.SaveChangesAsync();

            return (null, "Deck updated successfully");
        }

        private (bool, string) EditDeckIsValid(PlayersEditDeckRequest request)
        {
            if (request == null)
            {
                return (false, "Error: no information was provided for updating a deck");
            }

            if (string.IsNullOrWhiteSpace(request.Name) == true)
            {
                return (false, "Error: the deck's name is mandatory");
            }

            if (request.CardIds == null || request.CardIds.Count == 0 || request.CardIds.Count != 5)
            {
                return (false, "Error: the NPC's deck can neither be empty nor contain fewer or more than 5 cards");
            }

            return (true, string.Empty);
        }

        internal async Task<(PlayersDeleteDeckResponse?, string)> DeleteDeck(PlayersDeleteDeckRequest request)
        {
            if (request.DeckId <= 0)
            {
                return (null, $"Error: Invalid Deck ID, ID cannot be empty or equal/lesser than 0");
            }

            var exists = await _daoDbContext
                                   .Decks
                                   .AnyAsync(a => a.Id == request.DeckId && a.IsDeleted == false);

            if (exists == false)
            {
                return (null, $"Error: Invalid Deck ID, the npc does not exist or is already deleted");
            }

            await _daoDbContext
                      .Decks
                      .Where(a => a.Id == request.DeckId)
                      .ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

            return (null, "Delete deleted successfully");
        }
        #endregion

        #region Players Battles
        public async Task<(PlayersNewBattleResponse?, string)> NewBattle(PlayersNewBattleRequest request)
        {
            if (request.SaveId <= 0)
            {
                return (null, "Error: invalid Save Id");
            }

            var saveDB = await _daoDbContext
                                   .Saves
                                   .FirstOrDefaultAsync(a => a.Id == request.SaveId);
            if (saveDB == null)
            {
                return (null, "Error: save not found.");
            }

            var random = new Random();

            var validNpcsIdsDB = await _daoDbContext
                                           .Npcs
                                           .Where(a => a.IsDeleted == false && a.Level <= saveDB.PlayerLevel + 1)
                                           .Select(a => new { a.Id, a.Name })
                                           .ToListAsync();

            if (validNpcsIdsDB == null || validNpcsIdsDB.Count == 0)
            {
                return (null, "Error: no valid NPC was found for this match");
            }

            var randomNpc = validNpcsIdsDB.OrderBy(a => random.Next()).FirstOrDefault();

            var newMatch = new Battle
            {
                SaveId = saveDB.Id,
                NpcId = randomNpc.Id
            };

            _daoDbContext.Add(newMatch);

            await _daoDbContext.SaveChangesAsync();

            var content = new PlayersNewBattleResponse
            {
                BattleId = newMatch.Id,
                NpcName = randomNpc.Name
            };

            return (content, "A new battle started successfully");
        }

        public async Task<(PlayersPlayBattleResponse?, string)> PlayBattle(PlayersPlayBattleRequest request)
        {
            var (isValid, message) = PlayIsValid(request);
            if (isValid == false)
            {
                return (null, message);
            }

            var battleDB = await _daoDbContext
                .Battles
                .Include(a => a.Npc)
                .ThenInclude(a => a.Deck)
                .ThenInclude(a => a.Card)
                .Include(a => a.Save)
                .ThenInclude(a => a.Decks.Where(a => a.Id == request.DeckId))
                .ThenInclude(a => a.SaveDeckEntries)
                .ThenInclude(a => a.SaveCardEntry)
                .ThenInclude(a => a.Card)
                .Where(a => a.Id == request.BattleId)
                .FirstOrDefaultAsync();

            if (battleDB == null)
            {
                return (null, "Error: invalid battle");
            }

            var content = new PlayersPlayBattleResponse();

            if (battleDB.IsFinished == true)
            {
                var revertJason = JsonSerializer.Deserialize<PlayersPlayBattleResponse>(battleDB.Results);

                content = new PlayersPlayBattleResponse
                {
                    Duels = revertJason.Duels,
                    WinnerId = revertJason.WinnerId,
                    WinnerName = revertJason.WinnerName,
                };

                return (content, "Attention! This a record of a battle that was already finished");
            }


            var playerDeckDB = battleDB.Save.Decks.FirstOrDefault(a => a.Id == request.DeckId);

            if (playerDeckDB == null || playerDeckDB.SaveDeckEntries.Count == 0 || playerDeckDB.IsDeleted == true)
            {
                return (null, "Error: invalid deck");
            }

            var playerCardsDB = battleDB.Save.Decks.SelectMany(a => a.SaveDeckEntries.Select(b => b.SaveCardEntry.Card)).ToList();
            var npcCardsDB = battleDB.Npc.Deck.Select(a => a.Card).ToList();

            var duels = new List<List<PlayersPlayBattle_DuelingCard>>();

            for (int i = 0; i < Constants.DeckSize; i++)
            {
                var playerCardFullPower = Helper.GetCardFullPower(playerCardsDB[i].Power, playerCardsDB[i].UpperHand, (int)playerCardsDB[i].Type, (int)npcCardsDB[i].Type);

                var npcCardFullPower = Helper.GetCardFullPower(npcCardsDB[i].Power, npcCardsDB[i].UpperHand, (int)npcCardsDB[i].Type, (int)playerCardsDB[i].Type);

                duels.Add(
                    new List<PlayersPlayBattle_DuelingCard>
                    {
                        new PlayersPlayBattle_DuelingCard
                        {
                            CardName = playerCardsDB[i].Name,
                            CardType = playerCardsDB[i].Type,
                            CardPower = playerCardsDB[i].Power,
                            CardUpperHand = playerCardsDB[i].UpperHand,
                            CardFullPower = playerCardFullPower,
                            DualResult = Helper.GetDuelingPoints(playerCardFullPower, npcCardFullPower),
                        },
                        new PlayersPlayBattle_DuelingCard
                        {
                            CardName = npcCardsDB[i].Name,
                            CardType = npcCardsDB[i].Type,
                            CardPower = npcCardsDB[i].Power,
                            CardUpperHand = npcCardsDB[i].UpperHand,
                            CardFullPower = npcCardFullPower,
                            DualResult = Helper.GetDuelingPoints(npcCardFullPower, playerCardFullPower),
                        },
                    });
            }

            var playerTotalPoints = duels.Select(a => a[0].DualResult).Sum();

            var npcTotalPoints = duels.Select(a => a[1].DualResult).Sum();

            var winner = battleDB.Save.Name;

            if (npcTotalPoints > playerTotalPoints)
            {
                winner = battleDB.Npc.Name;

                battleDB.Save.PlayerLevel += 0;
                battleDB.Save.Gold += 0;
                battleDB.Save.CountMatches++;
                battleDB.Save.CountDefeats++;
                battleDB.Winner = winner;
                battleDB.IsFinished = true;

                content.Duels = duels;
                content.WinnerName = $"The NPC {winner} wins";
                content.WinnerId = battleDB.NpcId;

                await _daoDbContext.SaveChangesAsync();

                return (content, "Results read sucessfully");
            };

            content.WinnerName = $"{winner} wins";

            if (battleDB.Save.PlayerLevel < battleDB.Npc.Level)
            {
                battleDB.Save.PlayerLevel = battleDB.Npc.Level;
            }
            battleDB.Save.Gold += 1;
            battleDB.Save.CountMatches++;
            battleDB.Save.CountVictories++;
            battleDB.Winner = battleDB.Save.Name;
            battleDB.IsFinished = true;

            var allNpcsIdsDB = _daoDbContext
                                 .Npcs
                                 .Select(a => a.Id)
                                 .ToList();

            content.Duels = duels;
            content.WinnerId = battleDB.SaveId;

            message = "Battle result evaluated sucessfully";

            if (battleDB.Save.AllNpcsDefeatedTrophy == false)
            {
                var allNpcsDefeated = await _daoDbContext
                                                .Battles
                                                .Where(a => a.Save.Id == battleDB.SaveId)
                                                .Select(a => a.NpcId)
                                                .ToListAsync();

                if (allNpcsIdsDB.All(a => allNpcsDefeated.Contains(a)) == true)
                {
                    message += " . . . All NPCs have been defeated, a new trophy was unlocked";
                    battleDB.Save.AllNpcsDefeatedTrophy = true;
                }
            }

            if (battleDB.Save.AllNpcsDefeatedTrophy == true)
            {
                var allNpcsDefeated = await _daoDbContext
                                                .Battles
                                                .Where(a => a.Save.Id == battleDB.SaveId)
                                                .Select(a => a.NpcId)
                                                .ToListAsync();

                if (allNpcsIdsDB.All(a => allNpcsDefeated.Contains(a)) == false)
                {
                    message += " . . . new NPCS yet to be defeat, your trophy was removed";
                    battleDB.Save.AllNpcsDefeatedTrophy = false;
                }

            }

            battleDB.Results = JsonSerializer.Serialize(content);
            await _daoDbContext.SaveChangesAsync();

            return (content, message);
        }
        public (bool, string) PlayIsValid(PlayersPlayBattleRequest request)
        {
            if (request == null)
            {
                return (false, "Error: no information provided");
            }

            if (request.BattleId <= 0)
            {
                return (false, "Error: invalid BattleId");
            }

            if (request.DeckId <= 0)
            {
                return (false, "Error: invalid DeckId");
            }

            return (true, string.Empty);
        }

        public async Task<(PlayersGetBattleResultsResponse?, string)> GetBattleResults(PlayersGetBattleResultsRequest request)
        {
            if (request.BattleId == null && request.SaveId == null || request.BattleId <= 0 && request.SaveId <= 0)
            {
                return (null, "Error: informing either a BattleId or a SaveId is mandatory");
            }

            var battleDB = await _daoDbContext
                                    .Battles
                                    .FirstOrDefaultAsync(a => a.Id == request.BattleId && a.IsFinished == true);
            if (battleDB == null || string.IsNullOrEmpty(battleDB.Results))
            {
                return (null, "Error: no battle results found");
            }

            var revertJason = JsonSerializer.Deserialize<PlayersPlayBattleResponse>(battleDB.Results);

            var content = new PlayersGetBattleResultsResponse
            {
                Duels = revertJason.Duels,
                WinnerId = revertJason.WinnerId,
                WinnerName = revertJason.WinnerName,
            };

            return (content, "Results listed successfully");
        }
        #endregion

        #region Players Boosters
        public async Task<(List<PlayersOpenBoosterResponse>?, string)> OpenBooster(PlayersOpenBoosterRequest request)
        {
            if (request.SaveId <= 0)
            {
                return (null, "Error: invalid SaveId");
            }

            var random = new Random();

            var saveDB = await _daoDbContext
                                   .Saves
                                   .Include(a => a.SaveCardEntries)
                                   .FirstOrDefaultAsync(a => a.IsDeleted == false && a.Id == request.SaveId);

            if (saveDB == null)
            {
                return (null, "Error: save not found");
            }
            if (saveDB.Gold < 5)
            {
                return (null, "Error: not enough gold pieces");
            }

            // Criar um HashSet para armazenar os IDs já salvos e melhorar a performance
            var playerSaveCardEntries = saveDB.SaveCardEntries
                                        .GroupBy(a => a.CardId)
                                        .ToDictionary(cardId => cardId.Key, cardCount => cardCount.Count());

            var cardsDB = await _daoDbContext
                                .Cards
                                .ToListAsync();

            if (cardsDB == null || cardsDB.Count == 0)
            {
                return (null, "Error: no cards available");
            }

            var booster = cardsDB.OrderBy(a => random.Next()).Take(3).ToList();

            if (booster == null || booster.Count == 0)
            {
                return (null, "Error: no booster available");
            }

            var content = new List<PlayersOpenBoosterResponse>();

            foreach (var card in booster.OrderByDescending(a => a.Id))
            {
                content.Add(new PlayersOpenBoosterResponse
                {
                    CardId = card.Id,
                    CardName = card.Name,
                    CardType = card.Type,
                    CardPower = card.Power,
                    CardUpperHand = card.UpperHand,
                });

                playerSaveCardEntries.TryGetValue(card.Id, out int currentCount);

                if (currentCount < 5)
                {
                    saveDB.SaveCardEntries.Add(new SaveCardEntry
                    {
                        SaveId = saveDB.Id,
                        CardId = card.Id,
                    });

                    playerSaveCardEntries[card.Id] = currentCount + 1;
                }
            }

            saveDB.CountBoosters++;
            saveDB.Gold -= 5;

            var message = "Booster oppened successfully";

            var playerCardsId = saveDB.SaveCardEntries.Select(a => a.CardId).ToList();

            if (saveDB.AllCardsCollectedTrophy == false && cardsDB.Select(a => a.Id).All(cardId => playerCardsId.Contains(cardId)) == true)
            {
                message += " . . .All cards have been collect, a new trophy was unlocked!";
                saveDB.AllCardsCollectedTrophy = true;
            }

            if (saveDB.AllCardsCollectedTrophy == true && cardsDB.Select(a => a.Id).All(cardId => playerCardsId.Contains(cardId)) == false)
            {
                message += " . . . there are new cards available to be collected, therefore you lost your All Cards Collected Trophy!";
                saveDB.AllCardsCollectedTrophy = false;
            }

            await _daoDbContext.SaveChangesAsync();

            return (content, message);
        }
        #endregion

        #region Players Stats
        public async Task<(PlayersGetStatsResponse?, string)> GetStats(PlayersGetStatsRequest request)
        {
            if (request.SaveId == 0)
            {
                return (null, "Error: invalid SaveId");
            }

            var saveDB = await _daoDbContext.Saves
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(a => a.Id == request.SaveId && a.IsDeleted == false);

            if (saveDB == null)
            {
                return (null, "Error: save not found");
            }

            var content = new PlayersGetStatsResponse
            {
                Name = saveDB.Name,
                Gold = saveDB.Gold,
                CountMatches = saveDB.CountMatches,
                CountVictories = saveDB.CountVictories,
                CountDefeats = saveDB.CountDefeats,
                CountBoosters = saveDB.CountBoosters,
                PlayerLevel = saveDB.PlayerLevel,
                AllCardsCollectedTrophy = saveDB.AllCardsCollectedTrophy,
                AllNpcsDefeatedTrophy = saveDB.AllNpcsDefeatedTrophy
            };

            return (content, "Save statistics read successfully");
        }
        #endregion
    }
}
