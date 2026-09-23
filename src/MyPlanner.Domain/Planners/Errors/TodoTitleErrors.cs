using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners.ValueObjects;

namespace MyPlanner.Domain.Planners.Errors
{
    public static class TodoTitleErrors
    {
        public static readonly Error TitleRequired = Error.Validation(
          "TodoTitle.TitleRequired",
          "Title cannot be empty.");

        public static readonly Error TitleTooLong = Error.Validation(
           "TodoTitle.TitleTooLong",
           $"Title cannot be longer than {TodoTitle.MaxLength} characters.");
    }
}
