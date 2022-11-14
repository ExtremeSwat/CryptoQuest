using CryptoQuestService.Models.Tableland.Entities;
using CryptoQuestService.Services.HttpClients;

namespace CryptoQuestService.Services
{
    /// <summary>
    /// Main OP service 
    /// </summary>
    internal class CryptoQuestOperationsService
    {
        private readonly TablelandHttpService _tablelandHttpService;


        public CryptoQuestOperationsService(TablelandHttpService tablelandHttpService)
            => _tablelandHttpService = tablelandHttpService;

        internal async Task<List<ChallengesTable>> GrabCurrentChallenges()
            => (await _tablelandHttpService.GrabCurrentChallenges()) ?? new List<ChallengesTable>();

        internal async Task<ChallengesTable?> GrabChallengeById(int id)
            => await _tablelandHttpService.GrabChallengeById(id);
    }
}
