﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedievalAutoBattler.Models.Entities
{
    [Table("playersSaves")]
    public class PlayerSave
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public int PlayerLevel { get; set; }
        public int Gold { get; set; }
        public int CountMatches { get; set; }
        public int CountVictories { get; set; }
        public int CountDefeats { get; set; }
        public int CountBoosters { get; set; }

        [InverseProperty("Save")]
        public List<PlayerCardEntry> PlayerCardEntries { get; set; }

        [InverseProperty("Save")]
        public List<Deck> Decks { get; set; }

        public bool AllCardsCollectedTrophy { get; set; } = false;
        public bool AllNpcsDefeatedTrophy { get; set; } = false;
       
        public bool IsDeleted { get; set; } = false;
    }
}
