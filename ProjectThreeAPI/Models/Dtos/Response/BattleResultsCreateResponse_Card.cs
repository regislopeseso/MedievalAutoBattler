using MedievalAutoBattler.Models.Enums;

namespace MedievalAutoBattler.Models.Dtos.Response
{
    public class BattleResultsCreateResponse_Card
    {
        public string Name {  get; set; }
        public int Power { get; set; }
        public int UpperHand {  get; set; }
        public int Level { get; set; }
        public CardType Type { get; set; }
    }
}
