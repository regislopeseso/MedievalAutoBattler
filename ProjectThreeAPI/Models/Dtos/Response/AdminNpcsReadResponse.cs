using MedievalAutoBattler.Models.Entities;

namespace MedievalAutoBattler.Models.Dtos.Response
{
    public class AdminNpcsReadResponse
    {
        public int Id { get; set; }
        public string Name {  get; set; }
        public string Description { get; set; }
        public List<AdminNpcReadResponse_Deck>? Deck { get; set; }
        public int Level { get; set; }
    }
}
