namespace MyPlanner.Domain.Common
{
    public sealed record Result<T> : Result, IFailureBuildable<Result<T>>
    {
        private readonly T? _value;

        public T Value => _value
            ?? throw new InvalidOperationException($"Cannot access Value of a '{Outcome}' result.");

        private Result(T? value, ResultOutcomeType resultOutcomeType, Error error)
            : base(resultOutcomeType, error)
        {
            if (resultOutcomeType == ResultOutcomeType.Success && value is null)
            {
                throw new ArgumentNullException(
                    nameof(value),
                    "A success result must contain a value.");
            }

            if (resultOutcomeType == ResultOutcomeType.Success && value is Error)
            {
                throw new ArgumentException(
                    "A success result cannot have a value of type Error.",
                    nameof(value));
            }

            _value = value;
        }

        public static Result<T> Success(T value)
            => new(value, ResultOutcomeType.Success, Error.None);

        public static new Result<T> NoOp()
            => new(default, ResultOutcomeType.NoOp, Error.None);

        public static new Result<T> Failure(Error error)
            => new(default, ResultOutcomeType.Failure, error);

        public static implicit operator Result<T>(Error error)
            => Failure(error);
    }
}
