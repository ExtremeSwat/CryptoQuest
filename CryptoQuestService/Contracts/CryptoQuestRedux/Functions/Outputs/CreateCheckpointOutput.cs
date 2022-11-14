using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace CryptoQuestService.Contracts.CryptoQuestRedux.Functions.Outputs
{
    [FunctionOutput]
    public class CreateCheckpointOutput : IFunctionOutputDTO
    {
        [Parameter("uint256", 1)]
        public virtual BigInteger ChallengeCheckpointId { get; set; }
    }
}
