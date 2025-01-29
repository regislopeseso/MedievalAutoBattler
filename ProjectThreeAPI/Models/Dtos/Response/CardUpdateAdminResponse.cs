using ProjectThreeAPI.Models.Enums;

namespace ProjectThreeAPI.Models.Dtos.Response
{
    public class CardUpdateAdminResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Power { get; set; }
        public int UpperHand { get; set; }
        public CardType Type { get; set; }
    }
}
