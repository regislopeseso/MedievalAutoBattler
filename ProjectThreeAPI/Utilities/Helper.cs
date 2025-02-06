using MedievalAutoBattler.Models.Dtos.Request;
using MedievalAutoBattler.Models.Entities;

namespace MedievalAutoBattler.Utilities
{
    public static class Helper
    {
        public static int GetCardLevel(int power, int upperHand)
        {
            return (int)Math.Ceiling((double)(power + upperHand) / 2);
        }

        public static int GetNpcLevel(List<int> cardLevels)
        {
            return (int)Math.Ceiling((double)(cardLevels.Sum() / cardLevels.Count));
        }
    }
}
