using AutoMapper;
using CryptoQuestService.Contracts.CryptoQuestRedux.Functions;
using CryptoQuestService.Contracts.CryptoQuestRedux.Functions.Outputs;
using CryptoQuestService.Models.Dtos.Input;
using CryptoQuestService.Models.Settings;
using Microsoft.Extensions.Options;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace CryptoQuestService.Services.ContractInteraction
{
    public class CryptoQuestReduxIntegrationService
    {
        private readonly ApiSettings _apiSettings;
        private readonly IMapper _mapper;
        private readonly ILogger<CryptoQuestReduxIntegrationService> _logger;

        public CryptoQuestReduxIntegrationService(IOptions<ApiSettings> apiSettings, IMapper mapper, ILogger<CryptoQuestReduxIntegrationService> logger)
        {
            _apiSettings = apiSettings.Value;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task TriggerChallengeStart(int challengeId)
        {
            var web3 = GetWeb3Account();

            try
            {
                var contract = GetContractHandler(web3);
                var func = new TriggerChallengeStartFunction
                {
                    ChallengeId = challengeId   
                };

                var receipt = await contract.SendRequestAndWaitForReceiptAsync(func);
                if (!receipt.Succeeded())
                    throw new Exception("Failed to trigger the challenge start !");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error when trying to submit a challenge checkpoint");
                throw;
            }
        }

        public async Task<int> CreateChallengeCheckpoint(ChallengeCheckpointInputDto dto)
        {
            var web3 = GetWeb3Account();

            try
            {
                var contract = GetContractHandler(web3);
                var createCheckpointFunction = contract.GetFunction<CreateCheckpointFunction>();
                var chalengeCheckpoint = _mapper.Map<CreateCheckpointFunction>(dto);
                var data = await contract.QueryDeserializingToObjectAsync<CreateCheckpointFunction, CreateCheckpointOutput>(chalengeCheckpoint);

                return (int)data.ChallengeCheckpointId;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error when trying to submit a challenge checkpoint");
                throw;
            }
        }

        public async Task<int> CreateChallengeCheckpointTrigger(ChallengeCheckpointTriggerDto dto)
        {
            var web3 = GetWeb3Account();

            try
            {
                var contract = GetContractHandler(web3);
                var createCheckpointFunction = contract.GetFunction<CreateCheckpointTriggerFunction>();
                var checkpointTrigger = _mapper.Map<CreateCheckpointTriggerFunction>(dto);

                var data = await contract.QueryDeserializingToObjectAsync<CreateCheckpointTriggerFunction, CreateCheckpointTriggerOutput>(checkpointTrigger);
                return (int)data.ChallengeCheckpointTriggerId;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error when trying to submit a challenge checkpoint trigger");
                throw;
            }
        }

        private Web3 GetWeb3Account()
        {
            var deployerSettings = _apiSettings.AccountSettings;
            var chainSettings = _apiSettings.ChainSettings;

            var account = new Account(deployerSettings.PrivateKey, chainSettings.ChainId);
            return new Web3(account, _apiSettings.ChainSettings.ChainUri);
        }

        private ContractHandler GetContractHandler(Web3 web3)
            => web3.Eth.GetContractHandler(_apiSettings.ContractSettings.CryptoQuestReduxAddress);
    }
}
