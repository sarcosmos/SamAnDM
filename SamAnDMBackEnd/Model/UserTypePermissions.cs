namespace SamAnDMBackEnd.Model
{
    public class UserTypePermissions
    {
        public int UserTypeId { get; set; }
        public UserType UserType { get; set; }

        public int PermissionId { get; set; }
        public Permissions Permissions { get; set; }
    }
}
