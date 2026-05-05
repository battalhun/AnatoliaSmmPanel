using AnatoliaSmmPanel.Models;

namespace AnatoliaSmmPanel.Areas.Admin.Models;

public class AdminMenu
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // Menünün adı, kullanıcı arayüzünde gösterilecek metni belirtir
    public string NormalizedName => Name.ToUpperInvariant(); // NormalizedName, menünün adını büyük harfe çevirerek normalize eder, böylece karşılaştırmalar daha tutarlı hale gelir

    // NavigationTarget ile ilişki
    public int NavigationTargetId { get; set; } // NavigationTargetId, Menu ile NavigationTarget arasında bir ilişki olduğunu belirtir
    public NavigationTarget NavigationTarget { get; set; } = null!; // NavigationTarget nesnesi, Menu'nün hangi NavigationTarget'a yönlendirileceğini belirtir

    public bool IsActive { get; set; } // Menünün aktif olup olmadığını belirt

    public string? RequiredRole { get; set; } // Menünün görüntülenebilmesi için gerekli rolü belirtir

    public int Order { get; set; } // Menünün sıralamasını belirtir, daha düşük sayılar üstte görünür

    public string? Icon { get; set; } // Menünün yanında gösterilecek ikonun sınıf adını belirtir (örneğin, FontAwesome sınıfı)

    public bool IsAdminOnly { get; set; } // Menünün sadece admin kullanıcılar tarafından görülebilir olup olmadığını belirtir

    public bool OpenInNewTab { get; set; } // Menünün tıklandığında yeni bir sekmede açılıp açılmayacağını belirtir

    public ICollection<AdminMenuPermission> AdminMenuPermissions { get; set; } = new List<AdminMenuPermission>(); // AdminMenuPermissions, Menu'nün hangi izinlere sahip olduğunu belirtir, bu koleksiyon birden fazla izin içerebilir
}

