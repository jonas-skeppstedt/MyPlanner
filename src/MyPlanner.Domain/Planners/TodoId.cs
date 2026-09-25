namespace MyPlanner.Domain.Planners
{
    public readonly record struct TodoId
    {
        public Guid Value { get; }

        public TodoId(Guid value)
        {
            Value = value;
        }

        public static TodoId New() => new(Guid.NewGuid());

        public void ThrowIfEmpty(string paramName)
        {
            if (Value == Guid.Empty)
            {
                throw new ArgumentException("TodoId cannot be empty.", paramName);
            }
        }

        public override string ToString() => Value.ToString();
    }
}
