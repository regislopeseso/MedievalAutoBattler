using MedievalAutoBattler.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedievalAutoBattler.Models.Entities
{
    [Table("SaveDeckEntries")]
    public class SaveDeckEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [ForeignKey("Card")]
        public int CardId { get; set; }
        public required Card Card { get; set; }

        [InverseProperty(("SaveDeckEntries"))]
        public Deck Deck { get; set; }   // <-- Add this
    }
}
