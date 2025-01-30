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

        public static int GetNpcLevel(AdminNpcCreateRequest npc) 
        {
            //return (int)Math.Ceiling((double)(npc.Hand.Select(a => a.Level).Sum()) / 2);
            return 1;
        }
    }
}
