using System.Text.Json.Serialization;

namespace AnatoliaSmmPanel.Areas.Admin.Dtos
{
    // 2. Bakiye Sorgusu İçin
    public class SmmBalanceDto
    {
        [JsonPropertyName("balance")]
        public string Balance { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; } // Hata varsa buraya gelir
    }
}
