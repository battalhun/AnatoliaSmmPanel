using AnatoliaSmmPanel.Data.Models.Appliciton;
using AnatoliaSmmPanel.Areas.Admin.Enums;

namespace AnatoliaSmmPanel.Data.Models.Admin;

public class Ticket
{
    public int Id { get; set; }

    // Kullanıcı İlişkisi
    public int UserId { get; set; }
    public ApplicationUser User { get; set; }

    // Talep Detayları
    public string Subject { get; set; } = string.Empty;

    // Enum ile bilet durumu (Pending, Answered, Closed vb.)
    public TicketStatusEnum Status { get; set; } = TicketStatusEnum.Pending;

    // Yöneticiye bildirim göstermek için (Yeni mesaj geldiğinde true olur)
    public bool IsUnread { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Bilet altındaki mesajların geçmişi (Bire çok ilişki)
    public ICollection<TicketMessage> Messages { get; set; } = new List<TicketMessage>();
}