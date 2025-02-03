using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectThreeAPI.Models.Entities
{
    [Table("DeckEntries")]
    public class DeckEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Card")]
        public int? CardId { get; set; }
        public Card Card { get; set; }      
        public bool IsDeleted { get; set; }
        [InverseProperty("Deck")]
        public Npc Npc { get; set; }
    }
}
