namespace MedievalAutoBattler.Models.Dtos.Response.Players
{
    public class PlayersCreateBattleResponse
    {
        public int BattleId { get; set; }
        public required string NpcName { get; set; }
    }
}
