namespace MyPlanner.Domain.Shared
{
    public readonly record struct UserId
    {
        public Guid Value { get; }

        public UserId(Guid value)
        {
            Value = value;
        }

        public static bool TryParse(string? rawString, out UserId userId)
        {
            if (Guid.TryParse(rawString, out var guid))
            {
                userId = new UserId(guid);
                return true;
            }

            userId = default;
            return false;
        }

        public void ThrowIfEmpty(string paramName)
        {
            if (Value == Guid.Empty)
            {
                throw new ArgumentException("UserId cannot be empty.", paramName);
            }
        }

        public override string ToString() => Value.ToString();
    }
}
