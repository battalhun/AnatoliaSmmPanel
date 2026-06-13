using AnatoliaSmmPanel.Data.Models.Appliciton;

namespace AnatoliaSmmPanel.Data.Models.Admin;

public class TicketMessage
{
    public int Id { get; set; }

    // Hangi destek talebine ait olduğu (Ticket İlişkisi)
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }

    // Mesajı yazan kişi (Admin veya Kullanıcı ID'si)
    public int UserId { get; set; }
    public ApplicationUser User { get; set; }

    // Mesaj İçeriği
    public string Message { get; set; } = string.Empty;

    // Mesajı gönderen kişi Admin mi? (Arayüzde renkleri ve hizalamayı ayırt etmek için)
    public bool IsAdmin { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}