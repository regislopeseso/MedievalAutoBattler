
using MedievalAutoBattler.Models.Enums;

namespace MedievalAutoBattler.Models.Dtos.Request
{
    public class AdminCardsCreateRequest
    {
        public string Name { get; set; }
        public int Power { get; set; }
        public int UpperHand { get; set; }
        public CardType Type {get; set;}
    }
}
