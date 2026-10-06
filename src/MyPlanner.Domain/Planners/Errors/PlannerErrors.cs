using MyPlanner.Domain.Common;

namespace MyPlanner.Domain.Planners.Errors
{
    public static class PlannerErrors
    {
        public static readonly Error NotFound = Error.NotFound(
            "Planner.NotFound",
            "A planner with the provided ID was not found.");

        public static readonly Error TodoAlreadyExists = Error.Conflict(
            "Planner.TodoAlreadyExists",
            "The provided todo already exists in the planner.");

        public static readonly Error OnlyOwnerCanDelete = Error.Forbidden(
            "Planner.OnlyOwnerCanDelete ",
            "Only the owner can delete this planner.");
    }
}
