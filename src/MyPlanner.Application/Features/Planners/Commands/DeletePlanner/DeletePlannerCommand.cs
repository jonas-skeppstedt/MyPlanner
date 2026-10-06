using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Domain.Planners;

namespace MyPlanner.Application.Features.Planners.Commands.DeletePlanner
{
    public sealed record DeletePlannerCommand(PlannerId PlannerId)
        : ICommand, IRequireAuthentication;
}
