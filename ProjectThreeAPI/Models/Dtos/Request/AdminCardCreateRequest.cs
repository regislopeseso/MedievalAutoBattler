
using ProjectThreeAPI.Models.Enums;

namespace ProjectThreeAPI.Models.Dtos.Request
{
    public class AdminCardCreateRequest
    {
        public string Name { get; set; }
        public int Power { get; set; }
        public int UpperHand { get; set; }
        public CardType Type {get; set;}
    }
}
