using CryptoQuestService.Models.Tableland.Chain;
using CryptoQuestService.Services.HttpClients;
using Microsoft.Extensions.Caching.Memory;

namespace CryptoQuestService.Services.Caches
{
    public enum CryptoQuestTables
    {
        Mapskins,
        Users,
        Challenges,
        ChallengeCheckpoints,
        ChallengeCheckpointTriggers,
        Participants,
        ParticipantProgress
    }

    public class TablelandEntitiesCacheService
    {
        private readonly TablelandHttpService _tablelandHttpService;
        private readonly ILogger<TablelandEntitiesCacheService> _logger;
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 
        /// </summary>
        public TablelandEntitiesCacheService(TablelandHttpService tablelandHttpService, ILogger<TablelandEntitiesCacheService> logger, IMemoryCache memoryCache)
        {
            _tablelandHttpService = tablelandHttpService;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Initializes the db
        /// </summary>
        /// <returns></returns>
        public async Task InitializeTables()
        {
            _logger.LogInformation("Grabbing existing tables");

            var values = _memoryCache.Get<Dictionary<CryptoQuestTables, string>>(nameof(CryptoQuestTables));
            if (values != null)
                throw new Exception("Cache has already been initted");

            var enumValues = Enum.GetValues<CryptoQuestTables>();
            var tableNames = new Dictionary<CryptoQuestTables, OwnedTable>();

            try
            {
                var tables = await _tablelandHttpService.GrabCryptoQuestOwnedTables();
                tables.ForEach(t =>
                {
                    var enumValue = enumValues.First(ev => t.Name.StartsWith(ev.ToString(), StringComparison.OrdinalIgnoreCase));
                    tableNames.Add(enumValue, t);
                });

                _memoryCache.Set(nameof(CryptoQuestTables), tableNames);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception at grabbing data");
            }
        }

        public Dictionary<CryptoQuestTables, OwnedTable> GrabCurrentTables()
        {
            var tables = _memoryCache.Get<Dictionary<CryptoQuestTables, OwnedTable>>(nameof(CryptoQuestTables));
            if (!tables.Any())
                throw new Exception("Empty cache !");

            return tables;
        }
    }
}
