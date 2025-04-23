using MedievalAutoBattler.Models.Entities;

namespace MedievalAutoBattler.Models.Dtos.Request.Admins
{
    public class AdminsEditNpcRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> CardIds { get; set; }
    }
}
