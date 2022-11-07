namespace CryptoQuestService.Models.Settings
{
    public class AccountSettings
    {
        public string PrivateKey { get; set; } = default!;
        public int ChainId { get; set; }
    }
}
