namespace MedievalAutoBattler.Models.Dtos.Response
{
    public class BattlePlaysRunResponse
    {
        public List<List<BattlePlaysRunResponse_DuelingCard>> Duels { get; set; }     
        public int WinnerId { get; set; }
        public string Winner { get; set; }
    }
}
