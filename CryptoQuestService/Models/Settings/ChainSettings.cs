namespace CryptoQuestService.Models.Settings
{
    public class ChainSettings
    {
        /// <summary>
        /// Used for tableland network querying
        /// </summary>
        public string TablelandBaseUri { get; set; } = default!;
        public string ChainUri { get; set; } = default!;
        public string Registry { get; set; } = default!;
    }
}
