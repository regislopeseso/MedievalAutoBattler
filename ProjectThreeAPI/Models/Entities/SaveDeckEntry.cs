using ProjectThreeAPI.Models.Entities;
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
        public Card Card { get; set; }

        [ForeignKey("Save")]
        public int SaveId { get; set; }
        public Save Save { get; set; }
    }
}
