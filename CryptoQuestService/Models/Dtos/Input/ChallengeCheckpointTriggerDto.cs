namespace CryptoQuestService.Models.Dtos.Input
{
    public class ChallengeCheckpointTriggerDto
    {
        public int ChallengeId { get; set; }
        public int CheckpointId { get; set; }
        public string Title { get; set; }  = default!;
        public string ImageUrl { get; set; } = default!;
        public bool IsPhotoRequired { get; set; }
        public string PhotoDescription { get; set; } = default!;
        public bool IsUserInputRequired { get; set; }
        public string UserInputDescription { get; set; } = default!;
        public string UserInputAnswer { get; set; } = default!;
    }
}
