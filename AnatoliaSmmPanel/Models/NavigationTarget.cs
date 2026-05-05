using AnatoliaSmmPanel.Enums;

namespace AnatoliaSmmPanel.Models;

public class NavigationTarget
{
    public int Id { get; set; }

    // MVC routing 
    public string? Controller { get; set; } // MVC controller adı, menünün hangi controller'a yönlendireceğini belirtir
    public string? Action { get; set; } // MVC action adı, menünün hangi action'a yönlendireceğini belirtir

    // Identity için
    public string? Page { get; set; } // Identity sayfa adı, menünün hangi Identity sayfasına yönlendireceğini belirtir
    public string? Area { get; set; } // MVC area adı, menünün hangi area'ya yönlendireceğini belirtir (opsiyonel, sadece area kullanılıyorsa gerekli)

    // External link (opsiyonel)
    public string? Url { get; set; } // Menünün harici bir URL'ye yönlendirilmesi durumunda kullanılacak URL'yi belirtir

    // Partial view (opsiyonel)
    public string? PartialView { get; set; } // Menünün bir partial view olarak render edilmesi durumunda kullanılacak partial view adını belirtir

    // ENUM
    public MenuConnect MenuConnect { get; set; } // MenuConnect enum'ı, menünün türünü belirtir (örneğin, External link, Identity, vb.)

}