using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Domain.Planners;

namespace MyPlanner.Application.Features.Planners.Commands.ChangeTodoTitle
{
    public sealed record ChangeTodoTitleCommand(
        PlannerId PlannerId,
        TodoId TodoId,
        string NewTitle)
        : ICommand<ChangeTodoTitleResponse>, IRequireAuthentication;
}
