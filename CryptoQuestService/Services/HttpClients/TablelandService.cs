using CryptoQuestService.Models.Settings;
using Microsoft.Extensions.Options;

namespace CryptoQuestService.Services.HttpClients
{
    public class TablelandService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _options;

        private readonly Dictionary<CryptoQuestTables, string> _tableNames;
        private readonly string _cryptoQuestContractAddress;

        enum CryptoQuestTables
        {
            Mapskins,
            Users,
            Challenges,
            ChallengeCheckpoints,
            ChallengeCheckpointTriggers,
            Participants,
            ParticipantsProgress
        }

        public TablelandService(HttpClient httpClient, IOptions<ApiSettings> options)
        {
            _options = options.Value;
            var chainSettings = _options.ChainSettings;

            _cryptoQuestContractAddress = _options.ContractSettings.CryptoQuestAddress;

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(chainSettings.TablelandBaseUri);
        }

        public async Task GrabTableNames()
        {

        }
    }
}
