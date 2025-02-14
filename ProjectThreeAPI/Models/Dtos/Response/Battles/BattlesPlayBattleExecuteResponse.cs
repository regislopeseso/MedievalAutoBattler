namespace MedievalAutoBattler.Models.Dtos.Response.Battles
{
    public class BattlesPlayBattleExecuteResponse
    {
        public List<List<BattlesPlayBattleExecuteResponse_DuelingCard>> Duels { get; set; }
        public int WinnerId { get; set; }
        public string Winner { get; set; }
    }
}
