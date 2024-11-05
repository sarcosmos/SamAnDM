using SamAnDMBackEnd.Repository;

namespace SamAnDMBackEnd.Service
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionsAsync (int UserId, string permissionName);
    }
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<bool> HasPermissionsAsync(int UserId, string permissionName)
        {
            var permissions = await _permissionRepository.GetPermissionsByUserIdAsync(UserId);
            return permissions.Any(p => p.PermissionName == permissionName);
        }
    }
}
