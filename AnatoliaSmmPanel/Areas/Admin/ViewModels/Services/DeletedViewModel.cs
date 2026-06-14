namespace AnatoliaSmmPanel.Areas.Admin.ViewModels.Services
{
    public class DeletedViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; }
    }
}
