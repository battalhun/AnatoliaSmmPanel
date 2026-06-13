namespace AnatoliaSmmPanel.Areas.Admin.ViewModels.Tickets;

public class NewTicketViewModel
{
    public int UserId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}