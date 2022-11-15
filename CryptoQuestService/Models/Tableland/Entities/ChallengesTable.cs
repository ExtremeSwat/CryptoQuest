using CryptoQuestService.Models.Tableland.Entities.Enums;
using System.Text.Json.Serialization;

namespace CryptoQuestService.Models.Tableland.Entities
{
    public class ChallengesTable
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;

        public int FromTimestamp { get; set; }
        public int ToTimestamp { get; set; }

        public int? TriggerTimestamp { get; set; }

        public string UserAddress { get; set; } = default!;
        public int CreationTimestamp { get; set; }
        public int MapSkinId { get; set; }
        public int ChallengeStatus { get; set; }
        public string? WinnerAddress { get; set; }
        public string? ImagePreviewURL { get; set; }

        [JsonIgnore]
        public ChallengeStatus TypeOfChallengeStatus => (ChallengeStatus)ChallengeStatus;
    }
}
