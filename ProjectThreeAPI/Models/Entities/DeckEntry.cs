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
        public Card Card { get; set; }
    }
}
