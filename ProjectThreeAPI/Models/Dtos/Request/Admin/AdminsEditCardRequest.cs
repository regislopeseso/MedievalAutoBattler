using MedievalAutoBattler.Models.Enums;

namespace MedievalAutoBattler.Models.Dtos.Request.Admin
{
    public class AdminsEditCardRequest
    {
        public int CardId { get; set; }
        public string CardName { get; set; }
        public int CardPower { get; set; }
        public int CardUpperHand { get; set; }
        public CardType CardType { get; set; }
    }
}
