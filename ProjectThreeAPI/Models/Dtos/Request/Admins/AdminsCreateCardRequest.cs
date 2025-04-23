using MedievalAutoBattler.Models.Enums;

namespace MedievalAutoBattler.Models.Dtos.Request.Admins
{
    public class AdminsCreateCardRequest
    {
        public string CardName { get; set; }
        public int CardPower { get; set; }
        public int CardUpperHand { get; set; }
        public CardType CardType { get; set; }
    }
}
