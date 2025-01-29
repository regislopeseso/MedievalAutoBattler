﻿using ProjectThreeAPI.Models.Entities;

namespace ProjectThreeAPI.Models.Dtos.Request
{
    public class NpcCreateAdminRequest
    {  
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Card> Hand { get; set; }
    }
}
