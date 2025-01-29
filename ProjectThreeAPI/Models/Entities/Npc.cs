using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectThreeAPI.Models.Entities
{
    [Table("npcs")]
    public class Npc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [InverseProperty("Npcs")]
        public List<Card> Hand { get; set; }
        public int Level {  get; set; }
        public bool IsDeleted { get; set; }
    }
}
