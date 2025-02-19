using MedievalAutoBattler.Models.Enums;

namespace MedievalAutoBattler.Models.Dtos.Response.Admin
{
    public class AdminsFilterCardsResponse
    {
        public int CardId { get; set; }
        public string CardName { get; set; }
        public int CardPower { get; set; }
        public int CardUpperHand { get; set; }
        public int CardLevel { get; set; }
        public CardType CardType { get; set; }
    }
}
