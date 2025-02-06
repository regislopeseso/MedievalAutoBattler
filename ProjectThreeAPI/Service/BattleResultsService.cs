using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Dtos.Response;

namespace MedievalAutoBattler.Service
{
    public class BattleResultsService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public BattleResultsService(ApplicationDbContext daoDbContext)
        {
            this._daoDbContext = daoDbContext;
        }

        public async Task<(BattleResultsReadResponse?, string)> Read(BattleResultsReadRequest request)
        {
            return (null, "Results read sucessfully");
        }


    }
}
