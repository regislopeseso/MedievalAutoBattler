namespace MedievalAutoBattler.Models.Dtos.Response.Players
{
    public class PlayersGetBattleResultsResponse
    {
        public List<List<PlayersPlayBattle_DuelingCard>> Duels { get; set; }
        public int WinnerId { get; set; }
        public required string WinnerName { get; set; }
    }
}
