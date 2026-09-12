namespace MyPlanner.Domain.Common
{
    public sealed record Error
    {
        public static readonly Error None = new();

        public ErrorType Type { get; }
        public string Code { get; }
        public string Description { get; }

        // Private parameterless constructor for Error.None instance.
        private Error()
        {
            Type = ErrorType.None;
            Code = string.Empty;
            Description = string.Empty;
        }

        // Main private constructor enforcing error type invariants
        private Error(ErrorType errorType, string code, string description)
        {
            if (errorType == ErrorType.None)
            {
                throw new ArgumentException(
                    "ErrorType.None may only be used in Error.None",
                    nameof(errorType));
            }

            ArgumentException.ThrowIfNullOrWhiteSpace(code);
            ArgumentException.ThrowIfNullOrWhiteSpace(description);

            Type = errorType;
            Code = code;
            Description = description;
        }

        public static Error NotFound(string code, string description)
            => new(ErrorType.NotFound, code, description);

        public static Error Validation(string code, string description)
            => new(ErrorType.Validation, code, description);

        public static Error Conflict(string code, string description)
            => new(ErrorType.Conflict, code, description);

        public static Error Unauthorized(string code, string description)
            => new(ErrorType.Unauthorized, code, description);

        public static Error Forbidden(string code, string description)
            => new(ErrorType.Forbidden, code, description);

        public static Error Failure(string code, string description)
            => new(ErrorType.Failure, code, description);
    }
}
