using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnatoliaSmmPanel.Areas.Admin.Models;

public class Provider
{
    public int Id { get; set; }

    // Provider adı (ör: smmhell.com)
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    public bool AliasEnabled { get; set; }

    // Kullanıcıya görünen takma ad
    [MaxLength(150)]
    public string? Alias { get; set; }

    // API endpoint
    [Required]
    [MaxLength(500)]
    public string ApiUrl { get; set; } = string.Empty;

    // API key
    [Required]
    [MaxLength(500)]
    public string? ApiKey { get; set; } = string.Empty;

    // Bakiye bildirimi aktif mi
    public bool LowBalanceNotificationEnabled { get; set; }

    // Minimum bakiye limiti
    [Column(TypeName = "decimal(18,4)")]
    public decimal BalanceLimit { get; set; }

    // Güncel provider bakiyesi
    [Column(TypeName = "decimal(18,4)")]
    public decimal? CurrentBalance { get; set; }

    // Para birimi (USD, TRY vs)
    [MaxLength(10)]
    public string Currency { get; set; } = "USD";

    // Provider aktif mi
    public bool IsActive { get; set; } = true;

    // Son API kontrol zamanı
    public DateTime? LastCheckAt { get; set; }

    // Son başarılı senkronizasyon
    public DateTime? LastSyncAt { get; set; }

    // Oluşturulma tarihi
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Güncellenme tarihi
    public DateTime? UpdatedAt { get; set; }

    
}