using ProjectThreeAPI.Models.Entities;

namespace ProjectThreeAPI.Models.Dtos.Response
{
    public class NpcCreateAdminResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Card> Hand { get; set; }
        public int Level { get; set; }
    }
}

