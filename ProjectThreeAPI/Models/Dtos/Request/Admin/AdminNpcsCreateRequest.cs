using MedievalAutoBattler.Models.Entities;

namespace MedievalAutoBattler.Models.Dtos.Request.Admin
{
    public class AdminNpcsCreateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> CardIds { get; set; }
    }
}
