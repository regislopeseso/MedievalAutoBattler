﻿using ProjectThreeAPI.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectThreeAPI.Models.Entities
{
    [Table("Cards")]
    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Power { get; set; }
        public int UpperHand { get; set; }
        public int Level { get; set; }
        public CardType Type { get; set; }
        public bool IsDeleted { get; set; }
    }
}
