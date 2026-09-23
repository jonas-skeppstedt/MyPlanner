using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners.Errors;

namespace MyPlanner.Domain.Planners.ValueObjects
{
    public sealed record TodoTitle
    {
        public const int MaxLength = 25;

        public string Value { get; }

        private TodoTitle(string value)
        {
            Value = value;
        }

        public static Result<TodoTitle> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return TodoTitleErrors.TitleRequired;
            }

            var trimmedValue = value.Trim();

            if (trimmedValue.Length > MaxLength)
            {
                return TodoTitleErrors.TitleTooLong;
            }

            return Result<TodoTitle>.Success(new TodoTitle(trimmedValue));
        }

        public override string ToString() => Value;
    }
}
