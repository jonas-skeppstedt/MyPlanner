using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners.ValueObjects;

namespace MyPlanner.Domain.Planners.Errors
{
    public static class TodoDescriptionErrors
    {
        public static readonly Error DescriptionTooLong = Error.Validation(
            "TodoDescription.DescriptionTooLong",
            $"Description cannot be longer than {TodoDescription.MaxLength} characters.");
    }
}
