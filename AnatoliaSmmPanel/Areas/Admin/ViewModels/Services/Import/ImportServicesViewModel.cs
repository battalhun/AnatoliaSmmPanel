

namespace AnatoliaSmmPanel.Areas.Admin.ViewModels.Services.Import;

public class ImportServicesViewModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public string Rate { get; set; }

    public int Min { get; set; }

    public int Max { get; set; }

    public bool Dripfeed { get; set; }

    public bool Refill { get; set; }

    public bool Cancel { get; set; }


    public int ImportExternalServicesCategoryViewModelId { get; set; }
    public ImportExternalServicesCategoryViewModel ImportExternalServicesCategoryViewModel { get; set; }

}
