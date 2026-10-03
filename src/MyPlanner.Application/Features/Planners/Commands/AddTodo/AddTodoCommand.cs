using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Domain.Planners;

namespace MyPlanner.Application.Features.Planners.Commands.AddTodo
{
    public sealed record AddTodoCommand(PlannerId PlannerId, string Title)
        : ICommand<AddTodoResponse>, IRequireAuthentication;
}
