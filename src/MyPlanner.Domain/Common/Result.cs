namespace MyPlanner.Domain.Common
{
    public record Result
    {
        private readonly ResultOutcomeType _outcome;

        public bool IsSuccess => _outcome == ResultOutcomeType.Success;
        public bool IsNoOp => _outcome == ResultOutcomeType.NoOp;
        public bool IsFailure => _outcome == ResultOutcomeType.Failure;
        public Error Error { get; }

        protected Result(ResultOutcomeType resultOutcomeType, Error error)
        {
            ArgumentNullException.ThrowIfNull(error);

            if (resultOutcomeType == ResultOutcomeType.Failure && error == Error.None)
            {
                throw new ArgumentException(
                    "A failure result must contain an error.",
                    nameof(error));
            }

            if (resultOutcomeType != ResultOutcomeType.Failure && error != Error.None)
            {
                throw new ArgumentException(
                    "A success or noop result must contain Error.None.",
                    nameof(error));
            }

            _outcome = resultOutcomeType;
            Error = error;
        }

        public static Result Success()
            => new(ResultOutcomeType.Success, Error.None);

        public static Result NoOp()
            => new(ResultOutcomeType.NoOp, Error.None);

        public static Result Failure(Error error)
            => new(ResultOutcomeType.Failure, error);

        public static implicit operator Result(Error error)
            => Failure(error);
    }
}
