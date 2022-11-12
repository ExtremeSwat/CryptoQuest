using CryptoQuestService.Models.Deployment.CryptoQuest.Deployment;
using CryptoQuestService.Models.Settings;
using Microsoft.Extensions.Options;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace CryptoQuestService.Services
{
    public class ContractDeployer
    {
        private readonly ApiSettings _options;
        private readonly ILogger<ContractDeployer> _logger; 

        public ContractDeployer(IOptions<ApiSettings> options, ILogger<ContractDeployer> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task DeployCryptoQuest()
        {
            var web3 = GetWeb3Account();
            var cryptoQuestDeploymentMessage = new CryptoQuestDeployment()
            {
                Registry = "0xe7f1725E7734CE288F8367e1Bb143E90bb3F0512"
            };

            var deploymentHandler = web3.Eth.GetContractDeploymentHandler<CryptoQuestDeployment>();

            var transactionReceipt = await deploymentHandler.SendRequestAndWaitForReceiptAsync(cryptoQuestDeploymentMessage);
            var contractAddress = transactionReceipt.ContractAddress;

            _logger.LogInformation("Deployed contract at: {0}", contractAddress);
        }

        private Web3 GetWeb3Account()
        {
            var deployerSettings = _options.AccountSettings;
            var account = new Account(deployerSettings.PrivateKey, deployerSettings.ChainId);

            return new Web3(account, _options.ChainSettings.ChainUri);
        }
    }
}
