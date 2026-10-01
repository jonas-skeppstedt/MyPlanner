
using MyPlanner.Application.Abstractions.Messaging;

namespace MyPlanner.Application.Features.Planners.CreatePlanner
{
    public sealed record CreatePlannerCommand(string Title) : ICommand<CreatePlannerResponse>;
}
