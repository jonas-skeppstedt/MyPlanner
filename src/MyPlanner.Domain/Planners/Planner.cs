using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Planners.ValueObjects;
using MyPlanner.Domain.Shared;

namespace MyPlanner.Domain.Planners
{
    public sealed class Planner
    {
        private List<Todo> _todos = new();

        public PlannerId Id { get; private set; }
        public UserId OwnerId { get; private set; }
        public PlannerTitle Title { get; private set; }

        public IReadOnlyList<Todo> Todos => _todos;

        private Planner(PlannerId id, UserId ownerId, PlannerTitle title)
        {
            Id = id;
            OwnerId = ownerId;
            Title = title;
        }

        /// <summary>
        /// Used for EF Core materialization. Do not call directly.
        /// </summary>
        private Planner()
        {
            Title = default!;
        }

        public static Planner From(PlannerId id, UserId ownerId, PlannerTitle title)
        {
            id.ThrowIfEmpty(nameof(id));
            ownerId.ThrowIfEmpty(nameof(ownerId));
            ArgumentNullException.ThrowIfNull(title);

            return new Planner(id, ownerId, title);
        }

        public Result ChangeTitle(PlannerTitle newTitle)
        {
            ArgumentNullException.ThrowIfNull(newTitle);

            if (newTitle == Title)
            {
                return Result.NoOp();
            }

            Title = newTitle;

            return Result.Success();
        }

        public Result Delete(UserId requestedBy)
        {
            requestedBy.ThrowIfEmpty(nameof(requestedBy));

            if (requestedBy != OwnerId)
            {
                return PlannerErrors.OnlyOwnerCanDelete;
            }

            return Result.Success();
        }

        public Result AddTodo(Todo todo)
        {
            ArgumentNullException.ThrowIfNull(todo);

            if (_todos.Any(t => t.Id == todo.Id))
            {
                return PlannerErrors.TodoAlreadyExists;
            }

            _todos.Add(todo);

            return Result.Success();
        }

        public Result RemoveTodo(TodoId id)
        {
            id.ThrowIfEmpty(nameof(id));

            var todo = _todos.FirstOrDefault(t => t.Id == id);

            if (todo == null)
            {
                return TodoErrors.NotFound;
            }

            _todos.Remove(todo);

            return Result.Success();
        }
    }
}
