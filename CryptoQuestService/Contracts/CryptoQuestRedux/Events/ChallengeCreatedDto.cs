using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace CryptoQuestService.Contracts.CryptoQuestRedux.Events
{
    [Event("ChallengeCreated")]
    public class ChallengeCreatedDto : IEventDTO
    {
        [Parameter("uint256", "challengeId", 1, true)]
        public BigInteger ChallengeId { get; set; }

        [Parameter("address", "createdBy", 2, true)]
        public string CreatedBy { get; set; }
    }
}
