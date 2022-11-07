namespace CryptoQuestService.Models.Settings
{
    public class ApiSettings
    {
        public AccountSettings AccountSettings { get; set; } = default!;
        public ChainSettings ChainSettings { get; set; } = default!;
    }
}
