using MedievalAutoBattler.Models.Entities;

namespace MedievalAutoBattler.Models.Dtos.Response.Players
{
    public class PlayersGetCardsResponse
    {
        public Card Card { get; set; }
        public int Count { get; set; }
    }
}
