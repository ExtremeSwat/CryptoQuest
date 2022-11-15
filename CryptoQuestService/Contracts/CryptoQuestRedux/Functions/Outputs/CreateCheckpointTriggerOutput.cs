using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace CryptoQuestService.Contracts.CryptoQuestRedux.Functions.Outputs
{
    [FunctionOutput]
    public class CreateCheckpointTriggerOutput : IFunctionOutputDTO
    {
        [Parameter("uint256", 1)]
        public virtual BigInteger ChallengeCheckpointTriggerId { get; set; }
    }
}
