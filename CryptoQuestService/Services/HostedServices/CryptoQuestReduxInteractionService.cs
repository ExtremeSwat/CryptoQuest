﻿using CryptoQuestService.Contracts.CryptoQuestRedux.Events;
using CryptoQuestService.Contracts.CryptoQuestRedux.Functions;
using CryptoQuestService.Contracts.CryptoQuestRedux.Functions.Outputs;
using CryptoQuestService.Models.Settings;
using Microsoft.Extensions.Options;
using Nethereum.Web3;
using System.Numerics;

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
            var web3 = _contractDeployer.GetWeb3Account();
            // await EventListener(web3).ConfigureAwait(false);

            var challengeId = await SimulateChallengeCreation(web3).ConfigureAwait(false);
            var challengeCheckpointId = await SimulateChallengeCheckpointCreation(web3).ConfigureAwait(false);
            await SimulateChallengeCheckpointTrigger(web3, challengeId, challengeCheckpointId);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<int> SimulateChallengeCreation(Web3 web3)
        {
            var contract = web3.Eth.GetContractHandler(_apiSettings.ContractSettings.CryptoQuestReduxAddress);
            var createChallenge = new CreateChallengeFunction
            {
                Title = $"IOR HAHA - {DateTime.UtcNow.ToLongTimeString()}",
                Description = "IOR Description123",
                FromTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
                ToTimestamp = DateTimeOffset.Now.AddHours(24).ToUnixTimeSeconds(),
                MapSkinId = 1,
                ImagePreviewURL = "1"
            };

            var data = await contract.QueryDeserializingToObjectAsync<CreateChallengeFunction, CreateChallengeOutput>(createChallenge);
            return (int)data.ChallengeId;
        }

        private async Task<int> SimulateChallengeCheckpointCreation(Web3 web3)
        {
            var contract = web3.Eth.GetContractHandler(_apiSettings.ContractSettings.CryptoQuestReduxAddress);

            var createCheckpoint = new CreateCheckpointFunction
            {
                ChallengeId = 0,
                Order = BigInteger.Parse("4"),
                Title = "Test Title",
                IconUrl = "TestURL",
                IconId = 1,
                Lat = "69",
                Lng = "69"
            };

            var data = await contract.QueryDeserializingToObjectAsync<CreateCheckpointFunction, CreateCheckpointOutput>(createCheckpoint);
            return (int)data.ChallengeCheckpointId;
        }

        private async Task<int> SimulateChallengeCheckpointTrigger(Web3 web3, int challengeId, int challengeCheckpointId)
        {
            var contract = web3.Eth.GetContractHandler(_apiSettings.ContractSettings.CryptoQuestReduxAddress);

            var createCheckpointTrigger = new CreateCheckpointTriggerFunction
            {
                ChallengeId = challengeId,
                CheckpointId = challengeCheckpointId,
                Title = "Test Challenge Checkpoint Title",

                ImageUrl = "Test Challenge Checkpoint Trigger",
                IsPhotoRequired = true,

                PhotoDescription = "Test photo",
                IsUserInputRequired = true,

                UserInputDescription = "Test description",
                UserInputAnswer = "User input answer"
            };

            var data = await contract.QueryDeserializingToObjectAsync<CreateCheckpointTriggerFunction, CreateCheckpointTriggerOutput>(createCheckpointTrigger);
            return (int)data.ChallengeCheckpointTriggerId;
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
