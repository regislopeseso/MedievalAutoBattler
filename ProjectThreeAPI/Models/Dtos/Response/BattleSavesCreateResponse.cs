namespace MedievalAutoBattler.Models.Dtos.Response
{
    public class BattleSavesCreateResponse
    {
        public int BattleId { get; set; }
        public required string NpcName { get; set; }
    }
}
