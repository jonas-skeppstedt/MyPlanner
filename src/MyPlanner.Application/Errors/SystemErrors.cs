using MyPlanner.Domain.Common;

namespace MyPlanner.Application.Errors
{
    public static class SystemErrors
    {
        public static readonly Error UnexpectedError = Error.Failure(
            "System.UnexpectedError",
            "An unexpected error occurred.");
    }
}
