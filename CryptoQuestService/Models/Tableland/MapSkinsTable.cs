using System.Text.Json.Serialization;

namespace CryptoQuestService.Models.Tableland
{
    public class MapSkinsTable
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("skinName")]
        public string SkinName { get; set; }

        [JsonPropertyName("imagePreviewUrl")]
        public string ImagePreviewUrl { get; set; }

        [JsonPropertyName("mapUri")]
        public string MapUri { get; set; }
    }
}
