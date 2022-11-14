using CryptoQuestService.Models.Settings;
using CryptoQuestService.Models.Tableland.Chain;
using CryptoQuestService.Services.Caches;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Cmp;
using System.Text.Json;

namespace CryptoQuestService.Services.HttpClients
{
    internal class TablelandHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TablelandHttpService> _logger;
        private readonly TablelandEntitiesCacheService _tablelandEntitiesCacheService;
        private readonly ChainSettings _chainSettings;
        private readonly string _cryptoQuestContractAddress;

        private readonly JsonSerializerOptions _tablelandSerializerOptions;

        public TablelandHttpService(HttpClient httpClient, IOptions<ApiSettings> options, ILogger<TablelandHttpService> logger, TablelandEntitiesCacheService tablelandEntitiesCacheService)
        {
            _chainSettings = options.Value.ChainSettings;
            _cryptoQuestContractAddress = options.Value.ContractSettings.CryptoQuestAddress;

            _httpClient = httpClient;
            _logger = logger;
            _tablelandEntitiesCacheService = tablelandEntitiesCacheService;
            _httpClient.BaseAddress = new Uri(_chainSettings.TablelandBaseUri);

            _tablelandSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        internal async Task<List<OwnedTable>> GrabCryptoQuestOwnedTables()
        {
            var request = await _httpClient.GetAsync($"/chain/{_chainSettings.ChainId}/tables/controller/{_cryptoQuestContractAddress}");
            request.EnsureSuccessStatusCode();

            var stringContent = await request.Content.ReadAsStringAsync();
            var ownedTables = JsonSerializer.Deserialize<List<OwnedTable>>(stringContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (ownedTables is null)
            {
                var message = "Invaid parsing !";
                _logger.LogError(message);
                throw new Exception(message);
            }

            return ownedTables;
        }

        /// <summary>
        /// Returns a list of <see cref="Challenge"/>
        /// </summary>
        /// <returns></returns>
        internal async Task<List<Challenge>?> GrabCurrentChallenges()
        {
            var challengesTable = GrabTableByName(CryptoQuestTables.Challenges);
            var query = $"select * from {challengesTable.Name}";

            var content = await GrabDataFromRequestAsync(query);
            return JsonSerializer.Deserialize<List<Challenge>>(content, _tablelandSerializerOptions);
        }

        /// <summary>
        /// Grabs a single <see cref="Challenge"/> providing an id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal async Task<Challenge?> GrabChallengeById(int id)
        {
            var challengesTable = GrabTableByName(CryptoQuestTables.Challenges);
            var query = $"select * from {challengesTable.Name} where id={id}";

            var content = await GrabDataFromRequestAsync(query);
            return JsonSerializer.Deserialize<Challenge>(content, _tablelandSerializerOptions);
        }

        private async Task<string> GrabDataFromRequestAsync(string query)
        {
            var httpRequest = await _httpClient.GetAsync($"query?s={Uri.EscapeDataString(query)}");
            httpRequest.EnsureSuccessStatusCode();

            return await httpRequest.Content.ReadAsStringAsync();
        }

        private OwnedTable GrabTableByName(CryptoQuestTables tableName)
            => _tablelandEntitiesCacheService.GrabCurrentTables().First(f => f.Key == tableName).Value;
    }
}
