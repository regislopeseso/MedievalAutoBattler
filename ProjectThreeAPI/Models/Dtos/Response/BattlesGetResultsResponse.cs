namespace MedievalAutoBattler.Models.Dtos.Response
{
    public class BattlesGetResultsResponse
    {
        public string? Winner {  get; set; }
        public List<int>? BattleIds { get; set; }
    }
}
