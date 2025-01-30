using ProjectThreeAPI.Models.Entities;

namespace ProjectThreeAPI.Models.Dtos.Response
{
    public class AdminNpcReadResponse
    {
        public string Name {  get; set; }
        public string Description { get; set; }
        public List<HandOfCardsResponse>? Hand { get; set; }
        public int Level { get; set; }
    }
}
