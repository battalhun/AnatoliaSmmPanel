using System.Text.Json;
using System.Text.Json.Serialization;

namespace AnatoliaSmmPanel.Areas.Admin.Dtos
{
    public class SmmCancelOrderRequestDto
    {
        public string Key { get; set; } = string.Empty;
        public string Action { get; set; } = "cancel";

        // Birden fazla sipariş, virgülle ayrılmış string olarak gönderilir: "9,2,15"
        public string Orders { get; set; } = string.Empty;
    }

    public class SmmCancelOrderResponseDto
    {
        [JsonPropertyName("order")]
        public int Order { get; set; }

        // "cancel" alanı bazen sayı (1 = başarılı), bazen obje ({ "error": "..." })
        // olduğu için JsonElement ile esnek bırakılıyor.
        [JsonPropertyName("cancel")]
        public JsonElement Cancel { get; set; }

        // ── Yardımcı property'ler ────────────────────────────

        /// <summary>cancel alanı sayı ise iptal başarılı kabul edilir.</summary>
        [JsonIgnore]
        public bool IsSuccess => Cancel.ValueKind == JsonValueKind.Number;

        /// <summary>cancel alanı obje ve "error" içeriyorsa hata mesajı, yoksa null.</summary>
        [JsonIgnore]
        public string? ErrorMessage =>
            Cancel.ValueKind == JsonValueKind.Object &&
            Cancel.TryGetProperty("error", out var err)
                ? err.GetString()
                : null;
    }
}