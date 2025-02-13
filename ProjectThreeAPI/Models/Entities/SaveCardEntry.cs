using System.ComponentModel.DataAnnotations.Schema;

namespace MedievalAutoBattler.Models.Entities
{
    [Table("SaveCardEntries")]
    public class SaveCardEntry
    {
        public int Id { get; set; }


        [ForeignKey("Save")]
        public int SaveId { get; set; }
        [InverseProperty("SaveCardEntries")]
        public Save Save {  get; set; }

        [ForeignKey("Card")]
        public int CardId { get; set; }
        public Card Card { get; set; }
    }
}
