﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedievalAutoBattler.Models.Entities
{
    [Table("decks")]
    public class Deck
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }

        [InverseProperty("Deck")]
        public List<PlayerDeckEntry> PlayerDeckEntries { get; set; }
            
        [InverseProperty("Decks")]
        public PlayerSave Save { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
