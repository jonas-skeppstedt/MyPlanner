using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;

namespace MyPlanner.Application.Features.Planners.Commands.CreatePlanner
{
    public sealed record CreatePlannerCommand(string Title)
        : ICommand<CreatePlannerResponse>, IRequireAuthentication;
}
