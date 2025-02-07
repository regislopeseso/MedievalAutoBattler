using MedievalAutoBattler.Models.Entities;

namespace MedievalAutoBattler.Models.Dtos.Response
{
    public class BattleResultsReadResponse
    {
        public List<String> NpcCards { get; set; }
        public List<String> PlayerCards { get; set; }
        //public int PlayerScore { get; set; }
        //public int NpcScore { get; set; }
        //public string Winner {  get; set; }
        //public int LevelUp { get; set; }
        //public int Reward = 1;
    }
}
