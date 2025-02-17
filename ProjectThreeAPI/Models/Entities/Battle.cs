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

        [ForeignKey("Save")]
        public int SaveId { get; set; }     
        public PlayersSave Save {get; set;}       

        [ForeignKey("Npc")]
        public int NpcId {  get; set; }
        public Npc Npc { get; set; }

        public string? Winner { get; set; }
        public string? Results { get; set; }
        public bool IsFinished { get; set; } = false;
    }
}
