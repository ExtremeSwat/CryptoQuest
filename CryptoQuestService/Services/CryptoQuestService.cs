using AutoMapper;
using CryptoQuestService.Models.Dtos.Input;
using CryptoQuestService.Models.Tableland.Chain;
using CryptoQuestService.Models.Tableland.Entities;
using CryptoQuestService.Models.Tableland.Entities.Enums;
using CryptoQuestService.Services.Caches;
using CryptoQuestService.Services.ContractInteraction;
using CryptoQuestService.Services.HttpClients;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<List<ChallengesTable>> GetChallenges(int? challengeId = null)
        {
            var tableName = GrabTableByName(CryptoQuestTables.Challenges).Name;
            var values = new List<(string field, string value, bool inQuotes)>();
            if (challengeId.HasValue)
                values.Add(("id", challengeId.Value.ToString(), false));

            var currentChallenges = await _tablelandHttpService.GrabTableContents<ChallengesTable>(tableName, values);
            return currentChallenges != null && currentChallenges.Any() ? currentChallenges : new List<ChallengesTable>();
        }

        public async Task<List<ChallengeCheckpointTable>> GetChallengeCheckpoints(int challengeId, int? challengeCheckpointId = default)
        {
            var tableName = GrabTableByName(CryptoQuestTables.ChallengeCheckpoints).Name;
            var values = new List<(string field, string value, bool inQuotes)>(new[] { ("challengeId", challengeId.ToString(), false) });
            if (challengeCheckpointId.HasValue)
                values.Add(("id", challengeCheckpointId.Value.ToString(), false));

            return await _tablelandHttpService.GrabTableContents<ChallengeCheckpointTable>(tableName, values) ?? new List<ChallengeCheckpointTable>();
        }

        public async Task<int> CreateChallengeCheckpoint(ChallengeCheckpointInputDto dto)
        {
            var challenges = await GetChallenges(dto.ChallengeId);
            if (challenges[0].TypeOfChallengeStatus is ChallengeStatus.Finished or ChallengeStatus.Archived or ChallengeStatus.Published)
                throw new ArgumentException(nameof(dto.ChallengeId), "Challenge finished or in progress !");

            if (!challenges.Any())
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
