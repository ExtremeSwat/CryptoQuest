namespace CryptoQuestService.Models.Tableland.Entities
{
    public class ChallengeCheckpointTable
    {
        public int Id { get; set; }
        public int ChallengeId { get; set; }
        public int Ordering { get; set; }
        public int Title { get; set; }
        public string IconUrl { get; set; } = default!;
        public int IconId { get; set; }
        public int Lat { get; set; }
        public int Lng { get; set; }
        public int CreationTimestamp { get; set; }
    }
}
