using MyPlanner.Domain.Common;

namespace MyPlanner.Domain.Planners.Errors
{
    public static class TodoErrors
    {
        public static readonly Error NotFound = Error.NotFound(
            "Todo.NotFound",
            "A todo with the provided ID was not found.");
    }
}
