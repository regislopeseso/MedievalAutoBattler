﻿using ProjectThreeAPI.Models.Entities;

namespace ProjectThreeAPI.Models.Dtos.Request
{
    public class AdminNpcCreateRequest
    {  
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> Deck { get; set; }
    }
}
