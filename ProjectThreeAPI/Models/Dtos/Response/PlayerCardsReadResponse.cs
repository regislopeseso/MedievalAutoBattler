using MedievalAutoBattler.Models.Entities;

namespace MedievalAutoBattler.Models.Dtos.Response
{
    public class PlayerCardsReadResponse
    {
        public Card Card { get; set; }
        public int Count { get; set; }
    }
}
