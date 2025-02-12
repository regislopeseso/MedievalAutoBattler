using MedievalAutoBattler.Models.Enums;

namespace MedievalAutoBattler.Models.Dtos.Response
{
    public class BattlePlayResponse_DuelingCards
    {
        public string PlayerCardName { get; set; }
        public int PlayerCardPower { get; set; }
        public int PlayerCardUpperHand { get; set; }
        public int PlayerCardLevel { get; set; }
        public CardType PlayerCardType { get; set; }
        public int PlayerCardFullPower { get; set; }
        public int PlayerPoints { get; set; }

        public string NpcCardName { get; set; }
        public int NpcCardPower { get; set; }
        public int NpcCardUpperHand { get; set; }
        public int NpcCardLevel { get; set; }
        public CardType NpcCardType { get; set; }
        public int NpcCardFullPower { get; set; }
        public int NpcPoints { get; set; }
    }
}
