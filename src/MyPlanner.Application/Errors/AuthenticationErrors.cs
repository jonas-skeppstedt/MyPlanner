using MyPlanner.Domain.Common;

namespace MyPlanner.Application.Errors
{
    public static class AuthenticationErrors
    {
        public static readonly Error UserNotAuthenticated = Error.Unauthorized(
            "Authentication.UserNotAuthenticated",
            "This action require a authenticated user.");
    }
}
