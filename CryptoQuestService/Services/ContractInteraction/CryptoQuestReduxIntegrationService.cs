using AutoMapper;
using CryptoQuestService.Contracts.CryptoQuestRedux.Functions;
using CryptoQuestService.Models.Dtos.Input;
using CryptoQuestService.Models.Settings;
using Microsoft.Extensions.Options;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace CryptoQuestService.Services.ContractInteraction
{
    public class CryptoQuestReduxIntegrationService
    {
        private readonly ApiSettings _apiSettings;
        private readonly IMapper _mapper;

        public CryptoQuestReduxIntegrationService(IOptions<ApiSettings> apiSettings, IMapper mapper)
        {
            _apiSettings = apiSettings.Value;
            _mapper = mapper;
        }

        public async Task<int> CreateChallengeCheckpoint(ChallengeCheckpointInputDto dto)
        {
            var web3 = GetWeb3Account();

            var chalengeCheckpoint = _mapper.Map<CreateCheckpointFunction>(dto);
            var createCheckpointFunction = web3.Eth.GetContractTransactionHandler<CreateCheckpointFunction>();

            var receipt = await createCheckpointFunction.SendRequestAndWaitForReceiptAsync(_apiSettings.ContractSettings.CryptoQuestReduxAddress, chalengeCheckpoint);
            if (!receipt.Succeeded())
                throw new Exception("Tran failed");

            throw new Exception();
        }

        private Web3 GetWeb3Account()
        {
            var deployerSettings = _apiSettings.AccountSettings;
            var chainSettings = _apiSettings.ChainSettings;

            var account = new Account(deployerSettings.PrivateKey, chainSettings.ChainId);
            return new Web3(account, _apiSettings.ChainSettings.ChainUri);
        }
    }
}
