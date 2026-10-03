using MyPlanner.Domain.Planners;

namespace MyPlanner.Application.Features.Planners.Commands.AddTodo
{
    public sealed record AddTodoResponse(TodoId TodoId, string Title, string Description);
}
