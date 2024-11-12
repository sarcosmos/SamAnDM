using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SamAnDMBackEnd.Attributes;
using SamAnDMBackEnd.DTO;
using SamAnDMBackEnd.Model;
using SamAnDMBackEnd.Repository;
using SamAnDMBackEnd.Service;

namespace SamAnDMBackEnd.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public AuthController(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] UserRegisterDto userRegisterDto)
        {
            var result = await _authService.RegisterAsync(userRegisterDto);
            return Ok(new { message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] UserLoginDto userLoginDto)
        {
            var token = await _authService.LoginAsync(userLoginDto);

            // Obtener los detalles del usuario después de iniciar sesión
            var user = await _userRepository.GetByEmailAsync(userLoginDto.Email);

            return Ok(new { Token = token, UserName = user.Name });
        }

        [Authorize]
        [Permission("Auth")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers()
        {
            var users = await _authService.GetAllUsersAsync();
            return Ok(users);
        }

        [Authorize]
        [Permission("Auth")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsersById(int id)
        {
            var users = await _authService.GetUsersByIdAsync(id);
            if(users == null)
                return NotFound();

            return Ok(users);
        }

        [Authorize]
        [Permission("Auth")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            var user = await _authService.GetUsersByIdAsync(id);
            if (user == null)
                return NotFound();

            await _authService.SoftDeleteUsersAsync(id);
            return NoContent();
        }
    }
}
