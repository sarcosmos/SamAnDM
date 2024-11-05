using Microsoft.EntityFrameworkCore;
using SamAnDMBackEnd.Context;
using SamAnDMBackEnd.Model;

namespace SamAnDMBackEnd.Repository
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permissions>> GetPermissionsByUserIdAsync(int userId);
    }
    public class PermissionRepository : IPermissionRepository
    {
        private readonly DbContextDM _context;
        public PermissionRepository(DbContextDM context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Permissions>> GetPermissionsByUserIdAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.UserId == userId)
                .SelectMany(u => u.UserType.UserTypePermissions.Select(up => up.Permissions))
                .ToListAsync();
        }
    }
}
