using MedievalAutoBattler.Models.Entities;
using MedievalAutoBattler.Models.Enums;

namespace MedievalAutoBattler.Models.Dtos.Response.Players
{
    public class PlayersOpenBoosterResponse
    {
        public int CardId { get; set; }
        public string CardName { get; set; }
        public CardType CardType { get; set; }
        public int CardPower { get; set; }
        public int CardUpperHand { get; set; }
        public int CardLevel => (CardPower + CardUpperHand) / 2;
    }
}
