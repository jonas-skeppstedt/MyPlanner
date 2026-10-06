using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Domain.Planners;

namespace MyPlanner.Application.Features.Planners.Commands.RemoveTodo
{
    public sealed record RemoveTodoCommand(PlannerId PlannerId, TodoId TodoId)
        : ICommand, IRequireAuthentication;
}
