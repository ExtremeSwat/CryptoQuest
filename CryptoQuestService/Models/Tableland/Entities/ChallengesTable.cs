namespace CryptoQuestService.Models.Tableland.Entities
{
    public class ChallengesTable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int FromTimestamp { get; set; }
        public int ToTimestamp { get; set; }

        public int? TriggerTimestamp { get; set; }

        public string UserAddress { get; set; }
        public int CreationTimestamp { get; set; }
        public int MapSkinId { get; set; }
        public int ChallengeStatus { get; set; }
        public string WinnerAddress { get; set; }
        public string ImagePreviewURL { get; set; }
    }
}
