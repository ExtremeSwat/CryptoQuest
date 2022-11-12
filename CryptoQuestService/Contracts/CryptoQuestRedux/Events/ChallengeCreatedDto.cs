using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace CryptoQuestService.Contracts.CryptoQuestRedux.Events
{
    [Event("ChallengeCreated")]
    public class ChallengeCreatedDto : IEventDTO
    {
        [Parameter("uint256", "challengeId", 1)]
        public BigInteger ChallengeId { get; set; }

        [Parameter("uint256", "createdBy", 2)]
        public string CreatedBy { get; set; }
    }
}
