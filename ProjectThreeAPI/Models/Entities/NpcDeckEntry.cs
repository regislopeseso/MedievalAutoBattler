﻿using MedievalAutoBattler.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedievalAutoBattler.Models.Entities
{
    [Table("npcDeckEntries")]
    public class NpcDeckEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public int Id { get; set; }        
        [ForeignKey("Card")]
        public int CardId { get; set; }       
        public Card Card { get; set; }         
        [ForeignKey("Npc")]
        public int NpcId { get; set; }
        [InverseProperty("Deck")]
        public Npc Npc { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
