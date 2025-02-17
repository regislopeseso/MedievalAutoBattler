using System.ComponentModel.DataAnnotations.Schema;

namespace MedievalAutoBattler.Models.Entities
{
    [Table("PlayerCardEntries")]
    public class PlayersCardEntry
    {
        public int Id { get; set; }


        [ForeignKey("Save")]
        public int SaveId { get; set; }
        [InverseProperty("SaveCardEntries")]
        public PlayersSave Save {  get; set; }

        [ForeignKey("Card")]
        public int CardId { get; set; }
        public Card Card { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
