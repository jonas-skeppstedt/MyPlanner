using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Domain.Planners;

namespace MyPlanner.Application.Features.Planners.Commands.ChangeTodoDescription
{
    public sealed record ChangeTodoDescriptionCommand(
        PlannerId PlannerId,
        TodoId TodoId,
        string NewDescription)
        : ICommand<ChangeTodoDescriptionResponse>, IRequireAuthentication;
}
