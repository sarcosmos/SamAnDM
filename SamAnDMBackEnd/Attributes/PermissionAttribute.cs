using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SamAnDMBackEnd.Service;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SamAnDMBackEnd.Attributes
{
    public class PermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string _permission;

        public PermissionAttribute(string permission)
        {
            _permission = permission;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var logger = context.HttpContext.RequestServices.GetService<ILogger<PermissionAttribute>>();
            var permissionService = context.HttpContext.RequestServices.GetService<IPermissionService>();

            // Verificar que se haya obtenido el servicio de permisos
            if (permissionService == null)
            {
                logger?.LogError("Your services are not available.");
                context.Result = new ForbidResult();
                return;
            }

            // Extraer el userId desde el token JWT
            var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                logger?.LogWarning("User ID not found or is not valid in token claims.");
                context.Result = new ForbidResult();
                return;
            }

            logger?.LogInformation($"Checking permission '{_permission}' for user {userId}");

            // Verificar si el usuario tiene el permiso necesario
            if (!await permissionService.HasPermissionsAsync(userId, _permission))
            {
                logger?.LogWarning($"User {userId} does not have permission '{_permission}'.");
                context.Result = new ForbidResult();
            }
            else
            {
                logger?.LogInformation($"User {userId} has permission '{_permission}'.");
            }
        }
    }
}

