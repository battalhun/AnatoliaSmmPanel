namespace AnatoliaSmmPanel.ViewModels;

public class HomeTicketListItemViewModel
{
    public int Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsUnread { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
