
using ProjectThreeAPI.Models.Enums;

namespace ProjectThreeAPI.Models.Dtos.Response
{
    public class CardCreateAdminResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Power { get; set; }
        public int UpperHand { get; set; }
        public CardType Type { get; set; }
        public int Level { get; set; }
    }
}
