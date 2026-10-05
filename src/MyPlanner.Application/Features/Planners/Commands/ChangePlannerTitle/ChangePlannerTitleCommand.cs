using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Domain.Planners;

namespace MyPlanner.Application.Features.Planners.Commands.ChangePlannerTitle
{
    public sealed record ChangePlannerTitleCommand(PlannerId PlannerId, string NewTitle)
        : ICommand<ChangePlannerTitleResponse>, IRequireAuthentication;
}
