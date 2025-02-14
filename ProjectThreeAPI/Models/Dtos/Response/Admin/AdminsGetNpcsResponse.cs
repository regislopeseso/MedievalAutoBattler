﻿using MedievalAutoBattler.Models.Entities;

namespace MedievalAutoBattler.Models.Dtos.Response.Admin
{
    public class AdminsGetNpcsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AdminsGetNpcsResponse_Deck>? Deck { get; set; }
        public int Level { get; set; }
    }
}
