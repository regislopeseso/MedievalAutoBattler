using ProjectThreeAPI.Models.Dtos.Request;
using ProjectThreeAPI.Models.Entities;

namespace ProjectThreeAPI.Utilities
{
    public static class Helper
    {
        public static int GetCardLevel(AdminCardCreateRequest card)
        {
            return (int)Math.Ceiling((double)(card.Power + card.UpperHand) / 2);
        }

        public static int GetNpcLevel(Npc npc)
        {
            var cards = npc.Deck.Select(x => x.Card).ToList();
            var levelSum = cards.Sum(x => x.Level);

            if (levelSum == 0 || cards.Count == 0)
            {
                return 0;   
            }

            return (int)Math.Ceiling((double)(levelSum / cards.Count));
        }
    }
}
