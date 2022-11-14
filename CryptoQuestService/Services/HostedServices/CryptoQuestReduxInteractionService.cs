using CryptoQuestService.Contracts.CryptoQuestRedux.Events;
using CryptoQuestService.Contracts.CryptoQuestRedux.Functions;
using CryptoQuestService.Models.Settings;
using Microsoft.Extensions.Options;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace CryptoQuestService.Services.HostedServices
{
    public class CryptoQuestReduxInteractionService : IHostedService
    {
        private readonly ContractDeployer _contractDeployer;
        private readonly ILogger<CryptoQuestReduxInteractionService> _logger;
        private readonly ApiSettings _apiSettings;

        public CryptoQuestReduxInteractionService(ContractDeployer contractDeployer, ILogger<CryptoQuestReduxInteractionService> logger, IOptions<ApiSettings> options)
        {
            _contractDeployer = contractDeployer;
            _logger = logger;
            _apiSettings = options.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            return;
            var web3 = _contractDeployer.GetWeb3Account();
            // await EventListener(web3).ConfigureAwait(false);
            await SimulateChallengeCreation(web3).ConfigureAwait(false);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task SimulateChallengeCreation(Web3 web3)
        {
            var createChallengeFunction = web3.Eth.GetContractTransactionHandler<CreateChallengeFunction>();
            var createChallenge = new CreateChallengeFunction
            {
                Title = $"IOR HAHA - {DateTime.UtcNow.ToLongTimeString()}",
                Description = "IOR Description123",
                FromTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
                ToTimestamp = DateTimeOffset.Now.AddHours(24).ToUnixTimeSeconds(),
                MapSkinId = 1,
                ImagePreviewURL = "1"
            };

            var receipt = await createChallengeFunction.SendRequestAndWaitForReceiptAsync(_apiSettings.ContractSettings.CryptoQuestReduxAddress, createChallenge);

            if (!receipt.Succeeded())
                throw new Exception("Tran failed");

            var transferEventOutput = receipt.DecodeAllEvents<ChallengeCreatedDto>();
        }

        private async Task EventListener(Web3 web3)
        {

            //var evt = web3.Eth.GetEvent<ChallengeCreatedDto>("0x922D6956C99E12DFeB3224DEA977D0939758A1Fe");


            while (1 == 1)
            {

                await Task.Delay(1000);

                var contract = web3.Eth.GetContractHandler("0x922D6956C99E12DFeB3224DEA977D0939758A1Fe");

                var evt = contract.GetEvent<ChallengeCreatedDto>();

                //1 
                var changes = await evt.GetAllChangesAsync(evt.CreateFilterInput());

                //2
                //var getAll = evt.CreateFilterInput();

                //var changes = await evt.GetAllChangesAsync(getAll);


                changes.ForEach(change =>
                {
                    _logger.LogInformation("Challenge {Challenge} has been created by {CreatedBy}", change.Event.ChallengeId, change.Event.ChallengeId);
                });
            }
        }

        private async Task<decimal> GetAddressBalance(string address, Web3 web3)
        {
            var wei = await web3.Eth.GetBalance.SendRequestAsync(address);
            return Web3.Convert.FromWei(wei);
        }
    }
}
