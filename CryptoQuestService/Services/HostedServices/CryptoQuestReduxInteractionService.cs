using CryptoQuestService.Contracts.CryptoQuestRedux.Events;
using CryptoQuestService.Models.Deployment.CryptoQuestRedux.Deployment;

namespace CryptoQuestService.Services.HostedServices
{
    public class CryptoQuestReduxInteractionService : IHostedService
    {
        private readonly ContractDeployer _contractDeployer;

        public CryptoQuestReduxInteractionService(ContractDeployer contractDeployer)
            => _contractDeployer = contractDeployer;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var web3 = _contractDeployer.GetWeb3Account();
            var contract = web3.Eth.GetContract<CryptoQuestReduxDeployment>("0x0165878A594ca255338adfa4d48449f69242Eb8F");
            var evt = contract.GetEvent<ChallengeCreatedDto>();

            while (1 == 1)
            {
                await Task.Delay(1000);
                var changes = await evt.GetAllChangesAsync(evt.CreateFilterInput());
                changes.ForEach(change =>
                {
                    Console.WriteLine($"Challenge: {change.Event.ChallengeId} has been created by {change.Event.CreatedBy}");
                });
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
