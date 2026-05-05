using AnatoliaSmmPanel.Areas.Admin.ViewModels;

namespace AnatoliaSmmPanel.Areas.Admin.Services
{
    public interface ISettingsService
    {
        Task<GeneralSettingsViewModel> GetSettingsAsync();
        Task SaveSettingsAsync(GeneralSettingsViewModel settings);
    }
}
