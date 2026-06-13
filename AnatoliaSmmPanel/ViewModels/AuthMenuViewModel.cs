namespace AnatoliaSmmPanel.ViewModels;

public class AuthMenuViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty; // Menünün adı, kullanıcı arayüzünde gösterilecek metni belirtir

    public string? Icon { get; set; } // Menünün yanında gösterilecek ikonun sınıf adını belirtir (örneğin, FontAwesome sınıfı)

    public int Order { get; set; } // Menünün sıralamasını belirtir, daha düşük sayılar üstte görünür

    public bool IsVisible { get; set; } // Menünün kullanıcı arayüzünde görünür olup olmadığını belirtir

    public bool OpenInNewTab { get; set; } // Menünün tıklandığında yeni bir sekmede açılıp açılmayacağını belirtir

    public NavigationTargetViewModel NavigationTarget { get; set; } = null!; // NavigationTargetViewModel, Menu'nün hangi NavigationTarget'a yönlendirileceğini belirtir

    public List<MenuPermissionViewModel> MenuPermissions { get; set; } = new List<MenuPermissionViewModel>(); // MenuPermissions, menünün hangi izinlere sahip olduğunu belirtir
}
