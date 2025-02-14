using MedievalAutoBattler.Models.Enums;

namespace MedievalAutoBattler.Models.Dtos.Request.Admin
{
    public class AdminsCreateCardRequest
    {
        public string Name { get; set; }
        public int Power { get; set; }
        public int UpperHand { get; set; }
        public CardType Type { get; set; }
    }
}
