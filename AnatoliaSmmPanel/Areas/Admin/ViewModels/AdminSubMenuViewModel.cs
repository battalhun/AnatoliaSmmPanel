
using AnatoliaSmmPanel.Areas.Admin.ViewModels;
using AnatoliaSmmPanel.Models;

public class AdminSubMenuViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Icon { get; set; }  // Menünün yanında gösterilecek ikonun sınıf adını belirtir (örneğin, FontAwesome sınıfı)

    public int Order { get; set; }  // Menünün sıralamasını belirtir, daha düşük sayılar üstte görünür

    public bool IsVisible { get; set; } // Menünün kullanıcı arayüzünde görünür olup olmadığını belirtir

    public bool OpenInNewTab { get; set; } // Menünün tıklandığında yeni bir sekmede açılıp açılmayacağını belirtir

    public NavigationTargetViewModel NavigationTarget { get; set; } = null!; // NavigationTargetViewModel, menünün hangi sayfaya yönlendirileceğini belirtir

    public List<AdminSubMenuPermissionViewModel> AdminSubMenuPermissions { get; set; } = null!; // AdminSubMenuPermissions, menünün hangi roller tarafından görüntülenebileceğini belirtir

    public int AdminMenuId { get; set; } // Bu alt menünün hangi ana menüye ait olduğunu belirtir
}