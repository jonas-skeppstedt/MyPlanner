namespace MyPlanner.Domain.Common
{
    public sealed record Error
    {
        public ErrorType Type { get; }
        public string Code { get; }
        public string Description { get; }

        private Error(ErrorType errorType, string code, string description)
        {
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
