using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners.ValueObjects;

namespace MyPlanner.Domain.Planners.Errors
{
    public static class PlannerTitleErrors
    {
        public static readonly Error TitleRequired = Error.Validation(
            "PlannerTitle.TitleRequired",
            "Title cannot be empty.");

        public static readonly Error TitleTooLong = Error.Validation(
            "PlannerTitle.TitleTooLong",
            $"Title cannot be longer than {PlannerTitle.MaxLength} characters.");
    }
}
