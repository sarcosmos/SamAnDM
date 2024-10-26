using Microsoft.EntityFrameworkCore;
using SamAnDMBackEnd.Context;
using SamAnDMBackEnd.Model;

namespace SamAnDMBackEnd.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetAllUsersAsync();
        Task<Users> GetByEmailAsync(string eamil);
        Task<Users> GetUsersByIdAsync(int id);
        Task CreateUsersAsync(Users user);
        Task UpdateUsersAsync(Users user);
        Task SoftDeleteUsersAsync(int id);
        Task<Users> GetUserByPermissionsAsync(string name, string password);
    }
    public class UserRepository : IUserRepository
    {
        private readonly DbContextDM _context;
        public UserRepository(DbContextDM context)
        {
            _context = context;
        }

        public async Task CreateUsersAsync(Users user)
        {
                 _context.Users.Add(user);  
                await _context.SaveChangesAsync();  

        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
           return await _context.Users
                .Where(s => !s.IsDeleted)
                .ToListAsync();
        }

        public async Task<Users> GetByEmailAsync(string eamil)
        {
            return await _context.Users
                .Include(u => u.UserType)
                .ThenInclude(u => u.UserTypePermissions)
                .FirstOrDefaultAsync(u => u.Email == eamil);
        }

        public async Task SoftDeleteUsersAsync(int id)
        {
            var documents = await _context.Users.FindAsync(id);
            if (documents != null)
            {
                documents.IsDeleted = true;
                await _context.SaveChangesAsync();  
            }
        }

        public async Task UpdateUsersAsync(Users user)
        {
           var existingUser = await _context.Users.FindAsync(user.UserId); 

           if (existingUser != null)
           {
                existingUser.Email = user.Email;
                existingUser.Name = user.Name;
                existingUser.UserTypeId = user.UserId;
                existingUser.Password = user.Password;

                _context.Users.Update(existingUser);  
                await _context.SaveChangesAsync();  
           }
            else 
            {
                throw new KeyNotFoundException($"Usuario con ID {user.UserId} no encontrado.");
            }
        }

        public async Task<Users> GetUserByPermissionsAsync(string name, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == name);
            if (user == null || !VerifyPassword(password, user.Password))
            {
                return null;
            }

            return user;
        }

        private bool VerifyPassword(string inputPassword, string storedPassword)
        { 
            return inputPassword == storedPassword; 
        }

        public async Task<Users> GetUsersByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id && !u.IsDeleted);
        }
    }
}
