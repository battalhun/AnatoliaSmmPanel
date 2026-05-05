using AnatoliaSmmPanel.Areas.Admin.Services;
using AnatoliaSmmPanel.Areas.Admin.ViewModels;
using System.Text.Json;

public class SettingsService : ISettingsService
{
    private readonly string _filePath;

    public SettingsService()
    {
        // Dosyayı ana dizinde "settings.json" olarak tutuyoruz
        _filePath = Path.Combine(Directory.GetCurrentDirectory(), "settings.json");
    }

    public async Task<GeneralSettingsViewModel> GetSettingsAsync()
    {
        if (!File.Exists(_filePath))
            return new GeneralSettingsViewModel(); // Dosya yoksa boş model dön

        var json = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<GeneralSettingsViewModel>(json) ?? new GeneralSettingsViewModel();
    }

    public async Task SaveSettingsAsync(GeneralSettingsViewModel settings)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(settings, options);
        await File.WriteAllTextAsync(_filePath, json);
    }
}