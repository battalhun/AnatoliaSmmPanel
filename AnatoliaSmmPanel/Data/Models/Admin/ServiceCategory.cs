namespace AnatoliaSmmPanel.Data.Models.Admin
{
    public class ServiceCategory
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<SmmService> Services { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public int SortOrder { get; set; }
    }
}