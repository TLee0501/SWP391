using System.Security.Claims;

namespace SystemController
{
    public class Utils
    {
        public static string? GetUserIdFromHttpContext(HttpContext context)
        {
            var claim = context.User.FindFirst(ClaimTypes.Name);

            if (claim != null)
            {
                string userId = claim.Value;
                return userId;
            }

            return null;
        }

        public static string? GetUserRoleFromHttpContext(HttpContext context)
        {
            var claim = context.User.FindFirst(ClaimTypes.Role);

            if (claim != null)
            {
                string role = claim.Value;
                return role;
            }

            return null;
        }
    }
}

