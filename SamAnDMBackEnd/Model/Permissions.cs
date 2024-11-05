namespace SamAnDMBackEnd.Model
{
    public class Permissions
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }

        public ICollection<UserTypePermissions> UserTypePermissions;
    }
}
