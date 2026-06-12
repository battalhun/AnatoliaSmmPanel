namespace AnatoliaSmmPanel.Models
{
    public class AuthMenuPermission
    {
        public int Id { get; set; }

        public int AuthMenuId { get; set; }

        public AuthMenu AuthMenu  { get; set; } = null!;

        public string RoleName { get; set; } = string.Empty;
    }
}
