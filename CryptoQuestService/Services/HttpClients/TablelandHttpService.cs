using CryptoQuestService.Models.Settings;
using CryptoQuestService.Models.Tableland.Chain;
using CryptoQuestService.Models.Tableland.Entities;
using CryptoQuestService.Services.Caches;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CryptoQuestService.Services.HttpClients
{
    public class TablelandHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TablelandHttpService> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly ChainSettings _chainSettings;
        private readonly string _cryptoQuestContractAddress;

        private readonly JsonSerializerOptions _tablelandSerializerOptions;

        public TablelandHttpService(HttpClient httpClient, IOptions<ApiSettings> options, ILogger<TablelandHttpService> logger, IMemoryCache memory)
        {
            _chainSettings = options.Value.ChainSettings;
            _cryptoQuestContractAddress = options.Value.ContractSettings.CryptoQuestAddress;

            _httpClient = httpClient;
            _logger = logger;
            _memoryCache = memory;
            _httpClient.BaseAddress = new Uri(_chainSettings.TablelandBaseUri);

            _tablelandSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<List<OwnedTable>> GrabCryptoQuestOwnedTables()
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
        public async Task<List<ChallengesTable>?> GrabCurrentChallenges(string challengesTableName)
        {
            var query = $"select * from {challengesTableName}";

            var content = await GrabDataFromRequestAsync(query);
            return JsonSerializer.Deserialize<List<ChallengesTable>>(content, _tablelandSerializerOptions);
        }

        /// <summary>
        /// Grabs a single <see cref="Challenge"/> providing an id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ChallengesTable?> GrabChallengeById(int id, string challengesTableName)
        {
            var query = $"select * from {challengesTableName} where id={id}";

            var content = await GrabDataFromRequestAsync(query);
            return JsonSerializer.Deserialize<ChallengesTable>(content, _tablelandSerializerOptions);
        }

        private async Task<string> GrabDataFromRequestAsync(string query)
        {
            var httpRequest = await _httpClient.GetAsync($"query?s={Uri.EscapeDataString(query)}");
            httpRequest.EnsureSuccessStatusCode();

            return await httpRequest.Content.ReadAsStringAsync();
        }
    }
}
