using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System.Numerics;

namespace CryptoQuestService.Contracts.CryptoQuestRedux.Functions
{
    [Function("triggerChallengeStart")]
    public class TriggerChallengeStartFunction : FunctionMessage
    {
        [Parameter("uint256", "challengeId", 1)]
        public BigInteger ChallengeId { get; set; }
    }
}
