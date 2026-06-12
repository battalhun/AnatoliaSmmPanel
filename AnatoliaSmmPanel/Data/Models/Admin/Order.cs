using AnatoliaSmmPanel.Data.Models.Appliciton;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnatoliaSmmPanel.Data.Models.Admin;
public class Order
{
    public int Id { get; set; }

    // User
    public int UserId { get; set; }
    public ApplicationUser User { get; set; }

    // Service
    public int ServiceId { get; set; }
    public SmmService Service { get; set; }

    // Provider
    public int? ProviderId { get; set; }
    public Provider Provider { get; set; }

    public string Link { get; set; }
    public int Quantity { get; set; }
    public int StartCount { get; set; }
    public int Remains { get; set; }

    [Column(TypeName = "decimal(18,6)")]
    public decimal Charge { get; set; }

    [Column(TypeName = "decimal(18,6)")]
    public decimal ProviderCharge { get; set; }

    public string ExternalOrderId { get; set; }
    public string ProviderName { get; set; }

    public string Status { get; set; } = "Pending";
    // Pending, Awaiting, In progress, Processing, Completed, Partial, Canceled, Fail, Error

    public string Mode { get; set; } = "Auto";
    // Auto, Manual

    public bool IsRefillEnabled { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}