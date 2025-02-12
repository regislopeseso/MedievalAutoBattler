﻿using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace MedievalAutoBattler.Service
{
    public class BattleResultsService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public BattleResultsService(ApplicationDbContext daoDbContext)
        {
            this._daoDbContext = daoDbContext;
        }

        public async Task<(BattleResultsCreateResponse?, string)> Create(BattleResultsCreateRequest request)
        {
            if(request.BattleId <= 0)
            {
                return (null, "Error: invalid BattleId");
            }

            var battleDB = await this._daoDbContext
                                 .Battles
                                 .Include(a => a.Npc)
                                 .ThenInclude(b => b.Deck)
                                 .ThenInclude(c => c.Card)
                                 .Include(a => a.Save)
                                 .ThenInclude(b => b.Decks)
                                 .ThenInclude(c => c.SaveDeckEntries)
                                 .ThenInclude(d => d.Card)
                                 .FirstOrDefaultAsync(a => a.Id == request.BattleId);

            if (battleDB == null)
            {
                return (null, "Error: battle not found");
            }
            if(battleDB.IsFinished == true)
            {
                return (null, "Error: this battle is available only for reading");
            }

            var saveDB = battleDB.Save;
            var playerDeck = battleDB.PlayerDeck;
            var playerCardsDB = playerDeck.SaveDeckEntries.Select(a => a.Card).ToList();

            var npcDB = battleDB.Npc;
            var npcDeck = npcDB.Deck;
            var npcCardsDB = npcDeck.Select(a => a.Card).ToList();

            #region Como ficaram as cartas na mesa (5x5)
            var npcCards = new List<BattleResultsCreateResponse_Card>();
            foreach (var card in npcCardsDB)
            {
                npcCards.Add(
                    new BattleResultsCreateResponse_Card
                    {
                        Name = card.Name,
                        Power = card.Power,
                        UpperHand = card.UpperHand,
                        Level = card.Level,
                        Type = card.Type,
                    });
            }

            var playerCards = new List<BattleResultsCreateResponse_Card>();
            foreach (var card in playerCardsDB)
            {
                playerCards.Add(
                    new BattleResultsCreateResponse_Card
                    {
                        Name = card.Name,
                        Power = card.Power,
                        UpperHand = card.UpperHand,
                        Level = card.Level,
                        Type = card.Type,
                    });
            }

            var content = new BattleResultsCreateResponse();
            content.NpcCards = npcCards;
            content.PlayerCards = playerCards;
            #endregion

            #region Calcula o poder total de cada carta de ambos
            var npcCardsFullPower = new List<int>();

            for (int i = 0; i < npcCards.Count; i++)
            {
                npcCardsFullPower.Add(GetCardFullPower(npcCards[i].Power, npcCards[i].UpperHand, npcCards[i].Type, playerCards[i].Type));
            }

            content.NpcCardsFullPower = npcCardsFullPower;

            var playerCardsFullPower = new List<int>();

            for (int i = 0; i < playerCards.Count; i++)
            {
                playerCardsFullPower.Add(GetCardFullPower(playerCards[i].Power, playerCards[i].UpperHand, playerCards[i].Type, npcCards[i].Type));
            }

            content.PlayerCardsFullPower = playerCardsFullPower;
            #endregion

            #region Quantos pontos o NPC e o jogador respectivamente
            var npcScores = GetScores(npcCardsFullPower, playerCardsFullPower);
            content.NpcScores = npcScores;
            content.NpcFinalScore = npcScores.Sum();

            var playerScores = GetScores(playerCardsFullPower, npcCardsFullPower);
            content.PlayerScores = playerScores;
            content.PlayerFinalScore = playerScores.Sum();
            #endregion


            if (HasPlayerWon(content.NpcFinalScore, content.PlayerFinalScore) == false)
            {
                content.Winner = $"The NPC {npcDB.Name} wins";
                saveDB.PlayerLevel += 0;
                saveDB.Gold += 0;
                saveDB.CountMatches++;
                saveDB.CountDefeats++;
                battleDB.Winner = npcDB.Name;
                battleDB.IsFinished = true;

                await this._daoDbContext.SaveChangesAsync();

                return (content, "Results read sucessfully");
            }

            content.Winner = $"{saveDB.Name} wins";

            if (saveDB.PlayerLevel < npcDB.Level)
            {
                saveDB.PlayerLevel = npcDB.Level;
            }
            saveDB.Gold += 1;
            saveDB.CountMatches++;
            saveDB.CountVictories++;
            battleDB.Winner = saveDB.Name;
            battleDB.IsFinished = true;

            var allNpcsIdsDB = this._daoDbContext
                                 .Npcs
                                 .Select(a => a.Id)
                                 .ToList();

            var message = "Battle result evaluated sucessfully";

            if (saveDB.AllNpcsDefeatedTrophy == false)
            {
                var saveIdDB = saveDB.Id;
                var allNpcsDefeated = this._daoDbContext
                                            .Battles
                                            .Include(a => a.Npc)
                                            .Where(a => a.Save.Id == saveIdDB)
                                            .Select(a => a.Npc.Id)
                                            .ToList();                

                if (allNpcsIdsDB.All(a => allNpcsDefeated.Contains(a)) == true)
                {
                    message += " . . . All NPCs have been defeated, a new trophy was unlocked";
                    saveDB.AllNpcsDefeatedTrophy = true;
                }
            }

            await this._daoDbContext.SaveChangesAsync();

            return (content, message);
        }

        private static int GetCardFullPower(int power, int upperHand, CardType attackingType, CardType defendingType)
        {
            var cardFullPower = 0;

            if (attackingType == CardType.Archer && defendingType == CardType.Spearman || attackingType == CardType.Cavalry && defendingType == CardType.Archer || attackingType == CardType.Spearman && defendingType == CardType.Cavalry)
            {
                cardFullPower = power + upperHand;
            }
            else
            {
                cardFullPower = power;
            }

            return cardFullPower;
        }

        private static List<int> GetScores(List<int> attackingCardsFullPower, List<int> defendingCardsFullPower)
        {
            var attackingScores = new List<int>();

            for (int i = 0; i < attackingCardsFullPower.Count; i++)
            {
                if (attackingCardsFullPower[i] - defendingCardsFullPower[i] > 0)
                {
                    attackingScores.Add(1);
                }
                else
                {
                    attackingScores.Add(0);
                }
            }

            return attackingScores;
        }

        private bool HasPlayerWon(int npcFinalScore, int playerFinalScore)
        {
            if (npcFinalScore > playerFinalScore)
            {
                return false;
            }
            else if (npcFinalScore == playerFinalScore)
            {
                return false;
            }
            return true;
        }

        public async Task<(BattleResultsReadResponse?, string)> GetResults(BattleResultsReadRequest request)
        {
            if(request.BattleId <= 0)
            {
                return (null, "Error: invalid BattleId");
            }

            var battleDB = await this._daoDbContext
                                 .Battles                                 
                                 .FirstOrDefaultAsync(a => a.Id == request.BattleId);

            if (battleDB == null)
            {
                return (null, "Error: battle not found");
            }

            if(battleDB.IsFinished == false)
            {
                return (null, "Error: this battle is not yet finished");
            }

            var content = new BattleResultsReadResponse
            {
                Winner = battleDB.Winner
            };

            return (content, "Battle result read successfully");
        }
    }
}
