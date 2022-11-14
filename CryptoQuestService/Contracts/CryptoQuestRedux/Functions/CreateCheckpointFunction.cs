using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System.Numerics;


namespace CryptoQuestService.Contracts.CryptoQuestRedux.Functions
{
    [Function("createCheckpoint", "uint256")]
    public class CreateCheckpointFunction : FunctionMessage
    {
        [Parameter("uint256", "challengeId", 1)]
        public BigInteger ChallengeId { get; set; }

        [Parameter("uint256", "order", 2)]
        public BigInteger Order { get; set; }

        [Parameter("string", "title", 3)]
        public string Title { get; set; } = default!;

        [Parameter("string", "iconUrl", 4)]
        public string IconUrl { get; set; } = default!;

        [Parameter("uint256", "iconId", 5)]
        public BigInteger IconId { get; set; }

        [Parameter("string", "lat", 6)]
        public string Lat { get; set; } = default!;

        [Parameter("string", "lng", 7)]
        public string Lng { get; set; } = default!;
    }
}
