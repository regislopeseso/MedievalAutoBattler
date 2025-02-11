namespace MedievalAutoBattler.Models.Dtos.Response
{
    public class PlayerStatsReadResponse
    {
        public required string Name { get; set; }
        public int Gold {  get; set; }
        public int CountMatches { get; set; }
        public int CountVictories { get; set; }
        public int CountDefeats { get; set; }
        public int CountBoosters { get; set; }
        public int PlayerLevel { get; set; }
        public bool AllCardsCollectedTrophy { get; set; }
        public bool AllNpcsDefeatedTrophy { get; set; }
    }
}
