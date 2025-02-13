namespace MedievalAutoBattler.Models.Dtos.Response
{
    public class BattlesPlayBattleExecuteResponse
    {
        public List<List<BattlesPlayBattleExecuteResponse_DuelingCard>> Duels { get; set; }     
        public int WinnerId { get; set; }
        public string Winner { get; set; }
    }
}
