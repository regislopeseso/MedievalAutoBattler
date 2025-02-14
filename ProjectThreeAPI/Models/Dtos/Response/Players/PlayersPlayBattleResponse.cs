namespace MedievalAutoBattler.Models.Dtos.Response.Players
{
    public class PlayersPlayBattleResponse
    {
        public List<List<PlayersPlayBattle_DuelingCard>> Duels { get; set; }
        public int WinnerId { get; set; }
        public string Winner { get; set; }
    }
}
