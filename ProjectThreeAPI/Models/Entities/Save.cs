﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedievalAutoBattler.Models.Entities
{
    [Table("saves")]
    public class Save
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
        public required List<Deck> Decks { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
