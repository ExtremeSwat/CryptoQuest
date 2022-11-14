using CryptoQuestService.Models.Tableland.Chain;
using CryptoQuestService.Models.Tableland.Entities;
using CryptoQuestService.Services.Caches;
using CryptoQuestService.Services.HttpClients;

namespace CryptoQuestService.Services
{
    /// <summary>
    /// Main OP service 
    /// </summary>
    public class CryptoQuestOperationsService
    {
        private readonly TablelandHttpService _tablelandHttpService;
        private readonly TablelandEntitiesCacheService _tablelandEntitiesCacheService;

        public CryptoQuestOperationsService(TablelandHttpService tablelandHttpService, TablelandEntitiesCacheService tablelandEntitiesCacheService)
        {
            _tablelandHttpService = tablelandHttpService;
            _tablelandEntitiesCacheService = tablelandEntitiesCacheService;
        }

        public async Task<List<ChallengesTable>> GrabCurrentChallenges()
            => (await _tablelandHttpService.GrabCurrentChallenges(GrabTableByName(CryptoQuestTables.Challenges).Name) ?? new List<ChallengesTable>());

        public async Task<ChallengesTable?> GrabChallengeById(int id)
            => await _tablelandHttpService.GrabChallengeById(id, GrabTableByName(CryptoQuestTables.Challenges).Name);

        private OwnedTable GrabTableByName(CryptoQuestTables tableName)
        {
            var tables = _tablelandEntitiesCacheService.GrabCurrentTables();
            if (!tables.Any())
                throw new Exception("Empty cache !");

            return tables[tableName];
        }
    }
}
