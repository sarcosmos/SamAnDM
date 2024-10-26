namespace SamAnDMBackEnd.Model
{
    public class UserType
    {
        public int UserTypeId { get; set; }
        public string TypeName { get; set; }

        public ICollection<Users> Users { get; set; }
        public ICollection<UserTypePermissions> UserTypePermissions { get; set; } 
    }
}
