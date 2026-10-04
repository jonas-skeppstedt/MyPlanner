using MyPlanner.Domain.Planners.ValueObjects;

namespace MyPlanner.Domain.Planners
{
    public sealed class Todo
    {
        public TodoId Id { get; private set; }
        public TodoTitle Title { get; private set; }
        public TodoDescription Description { get; private set; } = TodoDescription.Empty;

        private Todo(TodoId id, TodoTitle title)
        {
            Id = id;
            Title = title;
        }

        /// <summary>
        /// Used for EF Core materialization. Do not call directly.
        /// </summary>
        private Todo()
        {
            Title = default!;
        }

        public static Todo From(TodoId id, TodoTitle title)
        {
            id.ThrowIfEmpty(nameof(id));
            ArgumentNullException.ThrowIfNull(title);

            return new Todo(id, title);
        }
    }
}
