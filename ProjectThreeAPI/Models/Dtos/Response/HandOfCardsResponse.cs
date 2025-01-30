using ProjectThreeAPI.Models.Enums;

namespace ProjectThreeAPI.Models.Dtos.Response
{
    public class HandOfCardsResponse
    {
        public string Name { get; set; }
        public int Power {  get; set; }
        public int UpperHand {  get; set; }
        public int Level { get; set; }
        public CardType Type { get; set; }        
    }
}
