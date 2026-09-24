using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners.Errors;

namespace MyPlanner.Domain.Planners.ValueObjects
{
    public sealed record TodoDescription
    {
        public const int MaxLength = 200;

        public string Value { get; }

        public static readonly TodoDescription Empty = new(string.Empty);

        private TodoDescription(string value)
        {
            Value = value;
        }

        public static Result<TodoDescription> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result<TodoDescription>.Success(Empty);
            }

            var trimmedValue = value.Trim();

            if (trimmedValue.Length > MaxLength)
            {
                return TodoDescriptionErrors.DescriptionTooLong;
            }

            return Result<TodoDescription>.Success(new TodoDescription(trimmedValue));
        }

        public override string ToString() => Value;
    }
}
