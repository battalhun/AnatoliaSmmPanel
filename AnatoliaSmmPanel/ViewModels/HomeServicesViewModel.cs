namespace AnatoliaSmmPanel.ViewModels;

public class HomeServicesViewModel
{
    public int Id { get; set; }

    // Kategori filtreleme için ID gerekiyor (0 = Uncategorized / General)
    public int CategoryId { get; set; }

    // Müşteriye kategori ID'si değil, doğrudan adını göstereceğiz
    public string CategoryName { get; set; }

    public string Name { get; set; }
    public string Type { get; set; }
    public decimal Rate { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }

    // Detay modalında gösterilecek açıklama
    public string? Description { get; set; }

    // Özellikleri ikonlarla göstermek için boolean değerler
    public bool Dripfeed { get; set; }
    public bool Refill { get; set; }
    public bool Cancel { get; set; }
}