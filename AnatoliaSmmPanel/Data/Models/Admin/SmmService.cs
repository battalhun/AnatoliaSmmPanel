using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnatoliaSmmPanel.Data.Models.Admin;

public class SmmService
{
    [Key]
    public int Id { get; set; }

    public int SortOrder { get; set; }

    // Provider (SMM panel provider)
    public int ProviderId { get; set; }
    public Provider Provider { get; set; }

    // Category (internal system category)
    public int? serviceCategoryId { get; set; }
    public ServiceCategory serviceCategory { get; set; }

    // Basic info
    [MaxLength(250)]
    public string Name { get; set; }

    [MaxLength(50)]
    public string Type { get; set; }

    [Column(TypeName = "decimal(18,4)")]
    public decimal Rate { get; set; }

    public int Min { get; set; }
    public int Max { get; set; }

    // External info
    public int? externalServiceInfoId { get; set; }
    public ExternalServiceInfo externalServiceInfo { get; set; }

    // Features
    public bool Dripfeed { get; set; }
    public bool Refill { get; set; }
    public bool Cancel { get; set; }

    // Status
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;

    // Sync tracking
    public DateTime? LastSyncAt { get; set; } 

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}