namespace MedievalAutoBattler.Models.Dtos.Request
{
    public class AdminNpcsFlexUpdateRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Dictionary<int, int>? DeckChanges { get; set; }     //<old ids, new ids>
    }
}
