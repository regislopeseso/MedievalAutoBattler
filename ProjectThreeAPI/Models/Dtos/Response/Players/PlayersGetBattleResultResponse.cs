namespace MedievalAutoBattler.Models.Dtos.Response.Players
{
    public class PlayersGetBattleResultResponse
    {
        public string? Winner { get; set; }
        public List<int>? BattleIds { get; set; }
    }
}
