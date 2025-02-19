using MedievalAutoBattler.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MedievalAutoBattler.Models.Entities
{
    [Table("playerDeckEntries")]
    public class PlayerDeckEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [ForeignKey("PlayerCardEntry")]
        public int PlayerCardEntryId { get; set; }
        public PlayerCardEntry PlayerCardEntry { get; set; }

        [InverseProperty(("PlayerDeckEntries"))]
        public Deck Deck { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
