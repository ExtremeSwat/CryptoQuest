namespace CryptoQuestService.Models.Settings
{
    public class ApiSettings
    {
        public ContractSettings ContractSettings { get; set; } = default!;
        public AccountSettings AccountSettings { get; set; } = default!;
        public ChainSettings ChainSettings { get; set; } = default!;
    }
}
