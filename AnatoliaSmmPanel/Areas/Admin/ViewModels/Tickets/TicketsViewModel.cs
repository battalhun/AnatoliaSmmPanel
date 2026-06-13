namespace AnatoliaSmmPanel.Areas.Admin.ViewModels.Tickets;

public class TicketsViewModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? UserNickname { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsUnread { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
