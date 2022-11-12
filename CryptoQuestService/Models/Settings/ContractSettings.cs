namespace CryptoQuestService.Models.Settings
{
    public class ContractSettings
    {
        /// <summary>
        /// OP address
        /// </summary>
        public string CryptoQuestReduxAddress { get; set; } = default!;

        /// <summary>
        /// DB operations address
        /// </summary>
        public string CryptoQuestAddress { get; set; } = default!;
    }
}
