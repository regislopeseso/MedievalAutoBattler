using MedievalAutoBattler.Models.Entities;

namespace MedievalAutoBattler.Models.Dtos.Request
{
    public class PlayerDecksUpdateRequest
    {
        public int Id {  get; set; }
        public string Name {  get; set; }
        public List<int> CardIds {  get; set; }
    }
}
    