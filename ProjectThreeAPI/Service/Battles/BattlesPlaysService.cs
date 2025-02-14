using MedievalAutoBattler.Models.Dtos.Request.Battles;
using MedievalAutoBattler.Models.Dtos.Response.Battles;
using MedievalAutoBattler.Models.Entities;
using MedievalAutoBattler.Models.Enums;
using MedievalAutoBattler.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection.Metadata;

namespace MedievalAutoBattler.Service.Battles
{
    public class BattlesPlaysService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public BattlesPlaysService(ApplicationDbContext daoDbContext)
        {
            _daoDbContext = daoDbContext;
        }

        public async Task<(BattlesPlayBattleExecuteResponse?, string)> Play(BattlesPlayBattleExecuteRequest request)
        {
            var (isValid, message) = RunIsValid(request);
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
                .Where(a => a.Id == request.BattleId && a.IsFinished == false)
                .FirstOrDefaultAsync();

            if (battleDB == null)
            {
                return (null, "Error: invalid battle");
            }

            var playerDeckDB = battleDB.Save.Decks.FirstOrDefault(a => a.Id == request.DeckId);
            if (playerDeckDB == null || playerDeckDB.SaveDeckEntries.Count == 0 || playerDeckDB.IsDeleted == true)
            {
                return (null, "Error: invalid deck");
            }

            var playerCardsDB = battleDB.Save.Decks.SelectMany(a => a.SaveDeckEntries.Select(b => b.SaveCardEntry.Card)).ToList();
            var npcCardsDB = battleDB.Npc.Deck.Select(a => a.Card).ToList();

            var duels = new List<List<BattlesPlayBattleExecuteResponse_DuelingCard>>();

            for (int i = 0; i < Constants.DeckSize; i++)
            {
                var playerCardFullPower = Helper.GetCardFullPower(playerCardsDB[i].Power, playerCardsDB[i].UpperHand, (int)playerCardsDB[i].Type, (int)npcCardsDB[i].Type);

                var npcCardFullPower = Helper.GetCardFullPower(npcCardsDB[i].Power, npcCardsDB[i].UpperHand, (int)npcCardsDB[i].Type, (int)playerCardsDB[i].Type);

                duels.Add(
                    new List<BattlesPlayBattleExecuteResponse_DuelingCard>
                    {
                        new BattlesPlayBattleExecuteResponse_DuelingCard
                        {
                            CardName = playerCardsDB[i].Name,
                            CardType = playerCardsDB[i].Type,
                            CardPower = playerCardsDB[i].Power,
                            CardUpperHand = playerCardsDB[i].UpperHand,
                            CardFullPower = playerCardFullPower,
                            DualResult = Helper.GetDuelingPoints(playerCardFullPower, npcCardFullPower),
                        },
                        new BattlesPlayBattleExecuteResponse_DuelingCard
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

            var content = new BattlesPlayBattleExecuteResponse();

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
                content.Winner = $"The NPC {winner} wins";
                content.WinnerId = battleDB.NpcId;

                await _daoDbContext.SaveChangesAsync();

                return (content, "Results read sucessfully");
            };

            content.Winner = $"{winner} wins";

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
            await _daoDbContext.SaveChangesAsync();

            return (content, message);
        }
        public (bool, string) RunIsValid(BattlesPlayBattleExecuteRequest request)
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
    }
}
