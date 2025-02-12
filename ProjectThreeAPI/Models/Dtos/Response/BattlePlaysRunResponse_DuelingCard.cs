using MedievalAutoBattler.Models.Enums;

namespace MedievalAutoBattler.Models.Dtos.Response
{
    public class BattlePlaysRunResponse_DuelingCard
    {
        public string CardName {  get; set; }
        public CardType CardType { get; set; }
        public int CardPower { get; set; }
        public int CardUpperHand { get; set; }
        public int CardFullPower { get; set; }
        public int DualResult { get; set; }
    }
}
