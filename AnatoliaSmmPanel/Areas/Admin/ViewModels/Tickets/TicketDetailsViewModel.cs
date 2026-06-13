using AnatoliaSmmPanel.Areas.Admin.Enums;
using AnatoliaSmmPanel.Data.Models.Admin;
using AnatoliaSmmPanel.Data.Models.Appliciton;

namespace AnatoliaSmmPanel.Areas.Admin.ViewModels.Tickets;

public class TicketDetailsViewModel
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public string? UserNickname { get; set; }
    public string Subject { get; set; } = string.Empty;
    public TicketStatusEnum Status { get; set; } = TicketStatusEnum.Pending;
    public bool IsUnread { get; set; } = true;
    public bool IsAdmin { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<TicketMessage> Messages { get; set; } = new List<TicketMessage>();  
}

