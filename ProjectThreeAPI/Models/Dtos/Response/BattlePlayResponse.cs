namespace MedievalAutoBattler.Models.Dtos.Response
{
    public class BattlePlayResponse
    {
        public List<List<BattlePlayResponse_DuelingCard>> Duels { get; set; }     
        public int WinnerId { get; set; }
        public string Winner { get; set; }
    }
}
