namespace AnatoliaSmmPanel.Areas.Admin.ViewModels
{
    public class SettingsViewModel
    {
        public GeneralSettingsViewModel General { get; set; } = new();

        public List<ProviderViewModel> Providers { get; set; } = new();


    }
}