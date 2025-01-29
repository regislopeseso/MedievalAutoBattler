using ProjectThreeAPI.Models.Dtos.Request;

namespace ProjectThreeAPI.Utilities
{
    public static class Helper
    {
        public static int GetCardLevel(CardCreateAdminRequest card)
        {
            return (int)Math.Ceiling((double)(card.Power + card.UpperHand) / 2);
        }
    }
}
