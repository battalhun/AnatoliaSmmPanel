using Microsoft.AspNetCore.Identity;

namespace AnatoliaSmmPanel.Data.Models.Appliciton;

public class ApplicationUser : IdentityUser
{
    public int UserId { get; set; }
    public string? Nickname { get; set; } 
    public decimal Balance { get; set; }
    public decimal Spent { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastAuthAt { get; set; }

    public decimal DiscountPercent { get; set; } = 0;
}
