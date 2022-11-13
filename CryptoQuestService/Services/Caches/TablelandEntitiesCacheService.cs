namespace CryptoQuestService.Services.Caches
{
    internal enum CryptoQuestTables
    {
        Mapskins,
        Users,
        Challenges,
        ChallengeCheckpoints,
        ChallengeCheckpointTriggers,
        Participants,
        ParticipantsProgress
    }

    public class TablelandEntitiesCacheService
    {
        private readonly Dictionary<CryptoQuestTables, string> _tableNames;

        public async Task InitializeTables()
        {

        }
    }
}
