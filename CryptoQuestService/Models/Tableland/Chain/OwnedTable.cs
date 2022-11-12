namespace CryptoQuestService.Models.Tableland.Chain
{
    /// <summary>
    /// /chain/{chainId}/tables/structure/{hash}
    /// <see cref="https://docs.tableland.xyz/rest-api#6119ca78a0d947748b6ff7a7c84e6a97"/>
    /// </summary>
    internal class OwnedTable
    {
        public string Controller { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Structure { get; set; } = default!;
    }
}
