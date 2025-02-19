using Microsoft.AspNetCore.Mvc;
using WineApi.Context;
using WineApi.Model;

namespace WineApi.Helpers
{
    public class AuthHelper
    {
        public AuthResults GetAuthenticatedUser(ControllerBase controller)
        {
            var userIdClaim = controller.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null)
            {
                return new AuthResults(false, null, controller.Unauthorized());
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return new AuthResults(false, null, controller.BadRequest("Invalid user ID in token."));
            }

            return new AuthResults(true, userId, null);
        }

        public async Task<bool> UserHasAdminRights(WineDbContext context, Guid? userId)
        {
            if (userId == null)
                return false;

            var user = await context.Users.FindAsync(userId);

            if (user == null)
                return false;

            return user.AdminRights;
        }

    }
}
