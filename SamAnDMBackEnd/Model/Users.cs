namespace SamAnDMBackEnd.Model
{
    public class Users
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; } = false;

        public int UserTypeId { get; set; } 
        public UserType UserType { get; set; }

        public ICollection<FamilyGroup> FamilyGroups { get; set; } 
    }
}
