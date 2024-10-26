using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SamAnDMBackEnd.DTO;
using SamAnDMBackEnd.Model;
using SamAnDMBackEnd.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SamAnDMBackEnd.Service
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(UserRegisterDto userRegisterDto);
        Task<string> LoginAsync(UserLoginDto userLoginDto);

        Task<IEnumerable<Users>> GetAllUsersAsync();
        Task<Users> GetUsersByIdAsync(int id);
        Task SoftDeleteUsersAsync(int id);
        Task UpdateUsersAsync(Users user);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<Users> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IPasswordHasher<Users> passwordHasher, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(UserRegisterDto userRegisterDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(userRegisterDto.Email);
            if (existingUser != null) throw new Exception("Usuario ya existe.");

            var user = new Users
            {
                Email = userRegisterDto.Email,
                Name = userRegisterDto.Name,
                UserTypeId = userRegisterDto.UserTypeId,
                Password = userRegisterDto.Password,
                IsDeleted = userRegisterDto.IsDeleted,
            };

            user.Password = _passwordHasher.HashPassword(user, userRegisterDto.Password);

            await _userRepository.CreateUsersAsync(user);
            return "Usuario registrado con éxito.";
        }

        public async Task<string> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await _userRepository.GetByEmailAsync(userLoginDto.Email);
            if (user == null) throw new Exception("Usuario no encontrado.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, userLoginDto.Password);
            if (result == PasswordVerificationResult.Failed) throw new Exception("Contraseña incorrecta.");

            // Generar el JWT Token
            var token = GenerateJwtToken(user);
            return token;
        }

        private string GenerateJwtToken(Users user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.UserType.TypeName)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task SoftDeleteUsersAsync(int id)
        {
            await _userRepository.SoftDeleteUsersAsync(id);
        }

        public async Task UpdateUsersAsync(Users user)
        {
            await _userRepository.UpdateUsersAsync(user);
        }

        public async Task<Users> GetUsersByIdAsync(int id)
        {
            return await _userRepository.GetUsersByIdAsync(id);
        }
    }

}
