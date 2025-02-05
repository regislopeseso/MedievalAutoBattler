using System.ComponentModel.DataAnnotations.Schema;

namespace MedievalAutoBattler.Models.Entities
{
    public class Deck
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        [InverseProperty("Deck")]
        public required List<SaveDeckEntry> DeckEntries { get; set; }
        [InverseProperty("Decks")]
        public Save Save { get; set; }     

    }
}
