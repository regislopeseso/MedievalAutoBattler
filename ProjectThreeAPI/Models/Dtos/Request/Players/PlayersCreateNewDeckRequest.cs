namespace MedievalAutoBattler.Models.Dtos.Request.Players
{
    public class PlayersCreateNewDeckRequest
    {
        public int SaveId { get; set; }
        public required string Name { get; set; }
        public List<int>? CardIds { get; set; }
    }
}
