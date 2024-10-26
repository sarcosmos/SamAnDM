using System.ComponentModel.DataAnnotations;

namespace SamAnDMBackEnd.DTO
{
    public class UserLoginDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
