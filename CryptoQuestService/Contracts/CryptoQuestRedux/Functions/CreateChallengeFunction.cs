using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System.Numerics;

namespace CryptoQuestService.Contracts.CryptoQuestRedux.Functions
{
    [Function("createChallenge", "uint256")]
    public class CreateChallengeFunction : FunctionMessage
    {
        [Parameter("string", "title", 1)]
        public string Title { get; set; } = default!;

        [Parameter("string", "description", 2)]
        public string? Description { get; set; }

        [Parameter("uint256", "fromTimestamp", 3)]
        public BigInteger FromTimestamp { get; set; }

        [Parameter("uint256", "toTimestamp", 4)]
        public BigInteger ToTimestamp { get; set; }

        [Parameter("uint256", "mapSkinId", 5)]
        public BigInteger MapSkinId { get; set; }

        [Parameter("string", "imagePreviewURL", 6)]
        public string? ImagePreviewURL { get; set; }
    }
}
