using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace CryptoQuestService.Contracts.CryptoQuestRedux.Functions.Outputs
{
    [FunctionOutput]
    public class CreateChallengeOutput : IFunctionOutputDTO
    {
        [Parameter("uint256", 1)]
        public virtual BigInteger ChallengeId { get; set; }
    }
}
