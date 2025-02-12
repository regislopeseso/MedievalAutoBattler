namespace MedievalAutoBattler.Models.Dtos.Response
{
    public class BattlePlaysGetResponse
    {
        public string? Winner {  get; set; }
        public List<int>? BattleIds { get; set; }
    }
}
