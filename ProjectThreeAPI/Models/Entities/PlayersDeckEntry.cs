using MedievalAutoBattler.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedievalAutoBattler.Models.Entities
{
    [Table("PlayerDeckEntries")]
    public class PlayersDeckEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [ForeignKey("SaveCardEntry")]
        public int SaveCardEntryId { get; set; }
        public PlayersCardEntry SaveCardEntry { get; set; }

        [InverseProperty(("SaveDeckEntries"))]
        public Deck Deck { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
