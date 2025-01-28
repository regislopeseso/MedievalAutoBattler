using ProjectThreeAPI.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectThreeAPI.Entities
{
    [Table("Cards")]
    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Power { get; set; }
        public int UpperHand { get; set; }
        public CardType Type {get; set;}
        public bool IsDeleted { get; set; }
    }
}
