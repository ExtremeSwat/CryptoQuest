using CryptoQuestService.Models.Settings;
using CryptoQuestService.Models.Tableland.Chain;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CryptoQuestService.Services.HttpClients
{
    internal class TablelandHttpService
    {
        private readonly HttpClient _httpClient;

        private readonly ChainSettings _chainSettings;
        private readonly string _cryptoQuestContractAddress;

        internal TablelandHttpService(HttpClient httpClient, IOptions<ApiSettings> options, ILogger<TablelandHttpService>)
        {
            _chainSettings = options.Value.ChainSettings;
            _cryptoQuestContractAddress = options.Value.ContractSettings.CryptoQuestAddress;

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_chainSettings.TablelandBaseUri);
        }

        internal async Task<List<OwnedTable>> GrabCryptoQuestOwnedTables()
        {
            var request = await _httpClient.GetAsync($"/chain/{_chainSettings.ChainId}/{_cryptoQuestContractAddress}");
            request.EnsureSuccessStatusCode();

            var stringContent = await request.Content.ReadAsStringAsync();
            var ownedTables = JsonSerializer.Deserialize<List<OwnedTable>>(stringContent);

            if (ownedTables is null)
                throw new Exception("Invalid parsing !");

            return ownedTables;
        }
    }
}
