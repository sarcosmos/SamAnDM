namespace SamAnDMBackEnd.Model
{
    public class Persons 
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public DateTime DateRegistration { get; set; }
        public string ProfilePicture { get; set; }
        public bool AccountStatus { get; set; }
        public DateTime LastLogin { get; set; }
        public string TokenRecovery { get; set; }
        public bool IsDeleted { get; set; } = false;

        public int UserId { get; set; } 
        public Users Users { get; set; }
    }
}
