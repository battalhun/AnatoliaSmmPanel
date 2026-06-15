namespace AnatoliaSmmPanel.ViewModels;

public class HomeOrderListItemViewModel
{
    public int Id { get; set; }

    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;

    public string Link { get; set; } = string.Empty;

    public int Quantity { get; set; }
    public int StartCount { get; set; }
    public int Remains { get; set; }

    public decimal Charge { get; set; }

    public string Status { get; set; } = string.Empty;

    public bool RefillAvailable { get; set; }
    public bool CancelAvailable { get; set; }

    public string? CancelReason { get; set; }

    public DateTime CreatedAt { get; set; }
}

