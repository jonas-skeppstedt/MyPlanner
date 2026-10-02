using MyPlanner.Domain.Common;

namespace MyPlanner.Domain.Planners.Errors
{
    public static class PlannerErrors
    {
        public static readonly Error NotFound = Error.NotFound(
            "Planner.NotFound",
            "A planner with the provided ID was not found.");
    }
}
