using MedievalAutoBattler.Models.Enums;

namespace MedievalAutoBattler.Models.Dtos.Request.Admins
{
    public class AdminsFilterCardsRequest
    {
        public string? CardName { get; set; }
        public int? StartCardId { get; set; }
        public int? EndCardId { get; set; }
        public int? CardPower { get; set; }
        public int? CardUpperHand { get; set; }
        public int? CardLevel { get; set; }
        public CardType? CardType { get; set; }
    }
}
