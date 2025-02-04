using MedievalAutoBattler.Models.Dtos.Request;

namespace MedievalAutoBattler.Service
{
    public class PlayerDecksService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public PlayerDecksService (ApplicationDbContext daoDbContext)
        {
            _daoDbContext = daoDbContext;
        }

        public async Task<string> Create(List<int> cards)
        {

            return "";
        }


    }
}