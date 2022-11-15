using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System.Numerics;

namespace CryptoQuestService.Contracts.CryptoQuestRedux.Functions
{
    [Function("createCheckpointTrigger", "uint256")]
    public class CreateCheckpointTriggerFunction : FunctionMessage
    {
        [Parameter("uint256", "challengeId", 1)]
        public BigInteger ChallengeId { get; set; }

        [Parameter("uint256", "checkpointId", 2)]
        public BigInteger CheckpointId { get; set; }

        [Parameter("string", "title", 3)]
        public string Title { get; set; } = default!;

        [Parameter("string", "imageUrl", 4)]
        public string ImageUrl { get; set; } = default!;

        [Parameter("bool", "isPhotoRequired", 5)]
        public bool IsPhotoRequired { get; set; }

        [Parameter("string", "photoDescription", 6)]
        public string PhotoDescription { get; set; } = default!;

        [Parameter("bool", "isUserInputRequired", 7)]
        public bool IsUserInputRequired { get; set; }

        [Parameter("string", "userInputDescription", 8)]
        public string UserInputDescription { get; set; } = default!;

        [Parameter("string", "userInputAnswer", 9)]
        public string UserInputAnswer { get; set; } = default!;
    }
}
