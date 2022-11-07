using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace CryptoQuestService.Models.Deployment.CryptoQuest.Functions
{
    /// <summary>
    /// Creates the base entities
    /// </summary>
    [Function("createBaseTables")]
    public class CreateBaseTablesFunction : FunctionMessage
    {
    }
}
