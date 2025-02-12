using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using MedievalAutoBattler.Models.Entities;
using MedievalAutoBattler.Models.Enums;
using MedievalAutoBattler.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace MedievalAutoBattler.Service
{
    public class BattlePlaysService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public BattlePlaysService(ApplicationDbContext daoDbContext)
        {
            this._daoDbContext = daoDbContext;
        }

        public async Task<(BattlePlayResponse?, string)> Play(BattlePlayRequest request)
        {
            var battleDB = await this._daoDbContext
                .Battles
                .Include(a => a.Npc)
                .ThenInclude(a => a.Deck)
                .ThenInclude(a => a.Card)
                .Include(a => a.Save)
                .ThenInclude(a => a.Decks.Where(a => a.Id == request.DeckId))
                .ThenInclude(a => a.SaveDeckEntries)
                .ThenInclude(a => a.Card)
                .Where(a => a.Id == request.BattleId && a.IsFinished == false)
                .FirstOrDefaultAsync();

            var playerCardsDB = battleDB.Save.Decks.SelectMany(a => a.SaveDeckEntries.Select(b => b.Card)).ToList();
            var npcCardsDB = battleDB.Npc.Deck.Select(a => a.Card).ToList();     
        
            var duels = new List<List<BattlePlayResponse_DuelingCard>>();

            for (int i = 0; i < Constants.DeckSize; i++)
            {
                var playerCardFullPower = Helper.GetCardFullPower(playerCardsDB[i].Power, playerCardsDB[i].UpperHand, (int)playerCardsDB[i].Type, (int)npcCardsDB[i].Type);

                var npcCardFullPower = Helper.GetCardFullPower(npcCardsDB[i].Power, npcCardsDB[i].UpperHand, (int)npcCardsDB[i].Type, (int)playerCardsDB[i].Type);

                duels.Add(
                    new List<BattlePlayResponse_DuelingCard>
                    {
                        new BattlePlayResponse_DuelingCard
                        {
                            CardName = playerCardsDB[i].Name,
                            CardType = playerCardsDB[i].Type,
                            CardPower = playerCardsDB[i].Power,
                            CardUpperHand = playerCardsDB[i].UpperHand,
                            CardFullPower = playerCardFullPower,
                            DualResult = Helper.GetDuelingPoints(playerCardFullPower, npcCardFullPower),
                        },
                        new BattlePlayResponse_DuelingCard
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

            var content = new BattlePlayResponse();

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

                await this._daoDbContext.SaveChangesAsync();

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

            var allNpcsIdsDB = this._daoDbContext
                                 .Npcs
                                 .Select(a => a.Id)
                                 .ToList();

            content.Duels = duels;
            content.WinnerId = battleDB.SaveId;

            var message = "Battle result evaluated sucessfully";

            if (battleDB.Save.AllNpcsDefeatedTrophy == false)
            {
                var saveIdDB = battleDB.Save.Id;
                var allNpcsDefeated = this._daoDbContext
                                            .Battles
                                            .Include(a => a.Npc)
                                            .Where(a => a.Save.Id == saveIdDB)
                                            .Select(a => a.Npc.Id)
                                            .ToList();

                if (allNpcsIdsDB.All(a => allNpcsDefeated.Contains(a)) == true)
                {
                    message += " . . . All NPCs have been defeated, a new trophy was unlocked";
                    battleDB.Save.AllNpcsDefeatedTrophy = true;
                }
            }
            await this._daoDbContext.SaveChangesAsync();

            return (content, message);
        }

        //public async Task<(BattleResultsReadResponse?, string)> GetResults(BattleResultsReadRequest request)
        //{
        //    if (request.BattleId <= 0)
        //    {
        //        return (null, "Error: invalid BattleId");
        //    }

        //    var battleDB = await this._daoDbContext
        //                         .Battles
        //                         .FirstOrDefaultAsync(a => a.Id == request.BattleId);

        //    if (battleDB == null)
        //    {
        //        return (null, "Error: duels not found");
        //    }

        //    if (battleDB.IsFinished == false)
        //    {
        //        return (null, "Error: this duels is not yet finished");
        //    }

        //    var content = new BattleResultsReadResponse
        //    {
        //        Winner = battleDB.Winner
        //    };

        //    return (content, "Battle result read successfully");
        //}

        //public (bool, string) CreateIsValid(BattlePlayersCreateRequest request)
        //{
        //    if (request == null)
        //    {
        //        return (false, "Error: no information provided");
        //    }

        //    if (request.BattleId <= 0)
        //    {
        //        return (false, "Error: invalid BattleId");
        //    }

        //    if (request.DeckId <= 0)
        //    {
        //        return (false, "Error: invalid DeckId");
        //    }

        //    return (true, String.Empty);
        //}
    }
}
