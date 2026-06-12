using System.Text.Json.Serialization;

namespace AnatoliaSmmPanel.Areas.Admin.Dtos
{
    // 1. Servis Listesi İçin
    public class SmmServiceDto
    {
        [JsonPropertyName("service")]
        public int Service { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("rate")]
        public string Rate { get; set; }

        [JsonPropertyName("min")]
        public int Min { get; set; }

        [JsonPropertyName("max")]
        public int Max { get; set; }

        [JsonPropertyName("dripfeed")]
        public bool Dripfeed { get; set; }

        [JsonPropertyName("refill")]
        public bool Refill { get; set; }

        [JsonPropertyName("cancel")]
        public bool Cancel { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }
    }
}
