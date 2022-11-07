namespace CryptoQuestService.Services.HostedServices
{
    public class DeployerHostedService : IHostedService
    {
        private readonly ILogger<DeployerHostedService> _logger;
        private readonly ContractDeployer _contractDeployer;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="contractDeployer"></param>
        public DeployerHostedService(ILogger<DeployerHostedService> logger, ContractDeployer contractDeployer)
        {
            _logger = logger;
            _contractDeployer = contractDeployer;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Preparing to deploy contracts");
            await _contractDeployer.DeployCryptoQuest();
            _logger.LogInformation("Deployed CryptoQUest");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping");
            return Task.CompletedTask;
        }
    }
}
