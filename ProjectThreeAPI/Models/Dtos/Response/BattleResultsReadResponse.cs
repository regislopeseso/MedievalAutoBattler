using MedievalAutoBattler.Models.Entities;

namespace MedievalAutoBattler.Models.Dtos.Response
{
    public class BattleResultsReadResponse
    {
        public List<BattleResultsReadResponse_Card> NpcCards { get; set; }
        public List<BattleResultsReadResponse_Card> PlayerCards { get; set; }

        public List<int> NpcCardsFullPower { get; set; }
        public List<int> PlayerCardsFullPower { get; set; }

        public List<int> NpcScores { get; set; }
        public List<int> PlayerScores { get; set;}

        public int NpcFinalScore { get; set; }
        public int PlayerFinalScore { get; set; }
        
        public string Winner {  get; set; }      
    }
}
