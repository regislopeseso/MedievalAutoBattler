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

        public static List<int> GetPowerSequence(int level, int pos)
        {
            var validCards = new List<int>();
            switch (pos)
            {
                case 1:
                    //Ex. level == 6
                    //(4, 4, 6, 8, 8 )
                    //level-2, level-2, level,   level+2, level+2
                    validCards.Add(level - 2);
                    validCards.Add(level - 2);
                    validCards.Add(level);
                    validCards.Add(level + 2);
                    validCards.Add(level + 2);
                    break;
                case 2:
                    //  Ex. level == 6
                    //  (4, 4, 7, 7, 8 )
                    //  level-2, level-2, level+1, level+1, level+2
                    validCards.Add(level - 2);
                    validCards.Add(level - 2);
                    validCards.Add(level + 1);
                    validCards.Add(level + 1);
                    validCards.Add(level + 2);
                    break;
                case 3:
                    //  Ex. level == 6
                    //  (4, 5, 5, 8, 8 )
                    //  level-2, level-1, level-1, level+2, level+2
                    validCards.Add(level - 2);
                    validCards.Add(level - 1);
                    validCards.Add(level + 1);
                    validCards.Add(level + 2);
                    validCards.Add(level + 2);
                    break;
                case 4:
                    //  Ex. level == 6
                    //  4, 5, 6, 7, 8
                    //  level-2, level-1, level, level+1, level+2
                    validCards.Add(level - 2);
                    validCards.Add(level - 1);
                    validCards.Add(level);
                    validCards.Add(level + 1);
                    validCards.Add(level + 2);
                    break;
                case 5:
                    //  Ex. level == 6
                    //  4, 5, 7, 7, 7
                    //  level-2, level-1, level+1, level+1, level+1
                    validCards.Add(level - 2);
                    validCards.Add(level - 1);
                    validCards.Add(level + 1);
                    validCards.Add(level + 1);
                    validCards.Add(level + 1);
                    break;
                case 6:
                    //  Ex. level == 6
                    //  4, 6, 6, 6, 8
                    //  level-2, level, level, level, level+2
                    validCards.Add(level - 2);
                    validCards.Add(level);
                    validCards.Add(level);
                    validCards.Add(level);
                    validCards.Add(level + 2);
                    break;
                case 7:
                    //  Ex. level == 6
                    //  4, 6, 6, 7, 7
                    //  level-2, level, level, level+1, level+1
                    validCards.Add(level - 2);
                    validCards.Add(level);
                    validCards.Add(level);
                    validCards.Add(level + 1);
                    validCards.Add(level + 1);
                    break;
                case 8:
                    //  Ex. level == 6
                    //  5, 5, 5, 7, 8
                    //  level-1, level-1, level-1, level+1, level+2
                    validCards.Add(level - 1);
                    validCards.Add(level - 1);
                    validCards.Add(level - 1);
                    validCards.Add(level + 1);
                    validCards.Add(level + 2);
                    break;
                case 9:
                    //  Ex. level == 6
                    //  5, 5, 6, 6, 8
                    //  level-1, level-1, level, level, level+2
                    validCards.Add(level - 1);
                    validCards.Add(level - 1);
                    validCards.Add(level);
                    validCards.Add(level);
                    validCards.Add(level + 2);
                    break;
                case 10:
                    //  Ex. level == 6
                    //  5, 5, 6, 7, 7
                    //  level-1, level-1, level, level+1, level+1
                    validCards.Add(level - 1);
                    validCards.Add(level - 1);
                    validCards.Add(level);
                    validCards.Add(level + 1);
                    validCards.Add(level + 1);
                    break;
                case 11:
                    //  //  Ex. level == 6
                    //  5, 6, 6, 6, 7
                    //  level-1, level, level, level, level+1 
                    validCards.Add(level - 1);
                    validCards.Add(level);
                    validCards.Add(level);
                    validCards.Add(level);
                    validCards.Add(level + 1);
                    break;
                default:
                    //  //  Ex. level == 6
                    //  6, 6, 6, 6, 6
                    //  level, level, level, level, level
                    validCards.Add(level);
                    validCards.Add(level);
                    validCards.Add(level);
                    validCards.Add(level);
                    validCards.Add(level); ;
                    break;
            }
            return validCards;
        }
    }
}
