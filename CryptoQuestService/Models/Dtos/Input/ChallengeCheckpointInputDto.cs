using System.ComponentModel.DataAnnotations;

namespace CryptoQuestService.Models.Dtos.Input
{
    public class ChallengeCheckpointInputDto
    {
        [Required]
        public int ChallengeId { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        public string Title { get; set; } = default!;
        public string? IconUrl { get; set; }
        public int IconId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
