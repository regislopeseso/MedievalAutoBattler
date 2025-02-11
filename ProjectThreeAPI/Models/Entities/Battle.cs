using MedievalAutoBattler.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;

namespace MedievalAutoBattler.Models.Entities
{
    [Table("battles")]
    public class Battle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required Save Save {get; set;}
        public Deck? PlayerDeck { get; set; }
        public Npc? Npc { get; set; }
        public string? Winner { get; set; }
        public bool IsFinished { get; set; } = false;
    }
}
