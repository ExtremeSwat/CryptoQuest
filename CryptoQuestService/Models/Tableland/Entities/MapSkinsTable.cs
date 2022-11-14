namespace CryptoQuestService.Models.Tableland.Entities
{
    public class MapSkinsTable
    {
        public int Id { get; set; }
        public string SkinName { get; set; } = default!;
        public string ImagePreviewUrl { get; set; } = default!;
        public string MapUri { get; set; } = default!;
    }
}
