namespace MedievalAutoBattler.Models.Dtos.Response
{
    public class BattleResultsGetResultsResponse
    {
        public string? Winner {  get; set; }
        public List<int>? BattleIds { get; set; }
    }
}
