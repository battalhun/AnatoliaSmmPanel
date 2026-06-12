using AnatoliaSmmPanel.Areas.Admin.Dtos;

namespace AnatoliaSmmPanel.Areas.Admin.ViewModels.Services.Import
{
    public class ImportViewModel
    {

        public List<ImportProviderViewModel> ımportProviderViewModel { get; set; } = new List< ImportProviderViewModel>();

        public List<ImportServicesViewModel> ImportServicesViewModel { get; set; } = new List<ImportServicesViewModel>();

        public int SelectedProviderId { get; set; }
    }
}
