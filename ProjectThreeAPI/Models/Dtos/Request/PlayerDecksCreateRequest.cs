namespace MedievalAutoBattler.Models.Dtos.Request
{
    public class PlayerDecksCreateRequest
    {
        public int SaveId { get; set; }
        public required string Name  { get; set; }
        public List<int> CardIds { get; set; }
    }
}
