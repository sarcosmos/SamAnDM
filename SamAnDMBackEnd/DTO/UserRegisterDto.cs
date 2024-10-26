namespace SamAnDMBackEnd.DTO
{
    public class UserRegisterDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public bool IsDeleted { get; set; } 

        public required int UserTypeId { get; set; }
    }
}
