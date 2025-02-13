using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;
using Microsoft.EntityFrameworkCore;

namespace MedievalAutoBattler.Service
{
    public class BattlesResultsService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public BattlesResultsService(ApplicationDbContext daoDBContext)
        {
            this._daoDbContext = daoDBContext;
        }

        public async Task<(BattlesGetResultsResponse?, string)> GetResults(BattlesGetResultsRequest request)
        {
            if (request.BattleId == null && request.SaveId == null || request.BattleId <= 0 && request.SaveId <= 0)
            {
                return (null, "Error: informing either a BattleId or a SaveId is mandatory");
            }


            var content = new BattlesGetResultsResponse();

            if (request.BattleId != null && request.BattleId > 0)
            {

                var battleWinnerDB = await this._daoDbContext
                                     .Battles
                                     .Where(a => a.Id == request.BattleId && a.IsFinished == true)
                                     .Select(a => a.Winner)
                                     .FirstOrDefaultAsync();

                if (string.IsNullOrEmpty(battleWinnerDB) == true)
                {
                    content.Winner = null;
                }
                else
                {
                    content.Winner = battleWinnerDB;
                }
            }

            if (request.SaveId != null && request.SaveId > 0)
            {

                var battleIdsDB = await this._daoDbContext
                                     .Battles
                                     .Where(a => a.SaveId == request.SaveId && a.IsFinished == true)
                                     .Select(a => a.Id)
                                     .ToListAsync();

                if (battleIdsDB == null || battleIdsDB.Count == 0)
                {
                    content.BattleIds = null;
                }
                else
                {
                    content.BattleIds = battleIdsDB;
                }
            }

            if (content.Winner == null && content.BattleIds == null)
            {
                return (null, "Error: no information found");
            }

            return (content, "Results listed successfully");
        }
    }
}
