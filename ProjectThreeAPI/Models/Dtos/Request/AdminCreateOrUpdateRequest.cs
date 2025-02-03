namespace MedievalAutoBattler.Models.Dtos.Request
{
    public class AdminCreateOrUpdateRequest
    {
        public string Name {  get; set; }
        public string Description { get; set; }
        public List<int> CardsId { get; set; }
    }
}
