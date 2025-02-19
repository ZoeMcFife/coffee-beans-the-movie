using Microsoft.AspNetCore.Mvc;
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
    }
}
