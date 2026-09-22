using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners.Errors;

namespace MyPlanner.Domain.Planners.ValueObjects
{
    public sealed record PlannerTitle
    {
        public const int MaxLength = 50;

        public string Value { get; }

        private PlannerTitle(string value)
        {
            Value = value;
        }

        public static Result<PlannerTitle> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return PlannerTitleErrors.TitleRequired;
            }

            var trimmedValue = value.Trim();

            if (trimmedValue.Length > MaxLength)
            {
                return PlannerTitleErrors.TitleTooLong;
            }

            return Result<PlannerTitle>.Success(new PlannerTitle(trimmedValue));
        }
    }
}
