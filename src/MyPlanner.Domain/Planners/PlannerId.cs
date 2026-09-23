namespace MyPlanner.Domain.Planners
{
    public readonly record struct PlannerId
    {
        public Guid Value { get; }

        public PlannerId(Guid value)
        {
            Value = value;
        }

        public static PlannerId New() => new(Guid.NewGuid());

        public void ThrowIfEmpty(string paramName)
        {
            if (Value == Guid.Empty)
            {
                throw new ArgumentException("PlannerId cannot be empty.", paramName);
            }
        }

        public override string ToString() => Value.ToString();
    }
}
