using AutoMapper;
using CryptoQuestService.Models.Dtos.Input;
using CryptoQuestService.Models.Tableland.Chain;
using CryptoQuestService.Models.Tableland.Entities;
using CryptoQuestService.Services.Caches;
using CryptoQuestService.Services.ContractInteraction;
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
        private readonly IMapper _mapper;
        private readonly CryptoQuestReduxIntegrationService _cryptoQuestReduxIntegrationService;

        public CryptoQuestOperationsService(TablelandHttpService tablelandHttpService, TablelandEntitiesCacheService tablelandEntitiesCacheService, IMapper mapper, CryptoQuestReduxIntegrationService cryptoQuestReduxIntegrationService)
        {
            _tablelandHttpService = tablelandHttpService;
            _tablelandEntitiesCacheService = tablelandEntitiesCacheService;
            _mapper = mapper;
            _cryptoQuestReduxIntegrationService = cryptoQuestReduxIntegrationService;
        }

        public async Task<List<ChallengesTable>> GrabCurrentChallenges()
            => (await _tablelandHttpService.GrabCurrentChallenges(GrabTableByName(CryptoQuestTables.Challenges).Name) ?? new List<ChallengesTable>());

        public async Task<ChallengesTable?> GrabChallengeById(int id)
            => await _tablelandHttpService.GrabChallengeById(id, GrabTableByName(CryptoQuestTables.Challenges).Name);

        public async Task<int> CreateChallengeCheckpoint(ChallengeCheckpointInputDto dto)
        {
            // We're only going to execute the query if the challenge actually exists lol
            var challenge = await GrabChallengeById(dto.ChallengeId);
            if (challenge is null)
                throw new ArgumentException(nameof(dto.ChallengeId), "Invalid ChallengeId");

            return await _cryptoQuestReduxIntegrationService.CreateChallengeCheckpoint(dto);
        }

        private OwnedTable GrabTableByName(CryptoQuestTables tableName)
        {
            var tables = _tablelandEntitiesCacheService.GrabCurrentTables();
            if (!tables.Any())
                throw new Exception("Empty cache !");

            return tables[tableName];
        }
    }
}
