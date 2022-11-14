using CryptoQuestService.Models.Settings;
using CryptoQuestService.Models.Tableland.Chain;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CryptoQuestService.Services.HttpClients
{
    internal class TablelandHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TablelandHttpService> _logger;
        private readonly ChainSettings _chainSettings;
        private readonly string _cryptoQuestContractAddress;

        public TablelandHttpService(HttpClient httpClient, IOptions<ApiSettings> options, ILogger<TablelandHttpService> logger)
        {
            _chainSettings = options.Value.ChainSettings;
            _cryptoQuestContractAddress = options.Value.ContractSettings.CryptoQuestAddress;

            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri(_chainSettings.TablelandBaseUri);
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
    }
}
