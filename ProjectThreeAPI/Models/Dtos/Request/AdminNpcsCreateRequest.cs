using ProjectThreeAPI.Models.Entities;

namespace ProjectThreeAPI.Models.Dtos.Request
{
    public class AdminNpcsCreateRequest
    {  
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> CardIds { get; set; }
    }
}
