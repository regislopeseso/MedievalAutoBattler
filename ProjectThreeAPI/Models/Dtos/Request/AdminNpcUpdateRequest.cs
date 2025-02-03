using ProjectThreeAPI.Models.Entities;

namespace ProjectThreeAPI.Models.Dtos.Request
{
    public class AdminNpcUpdateRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Dictionary<int,int>? DeckChanges { get; set; }     //<old ids, new ids>
    }
}
