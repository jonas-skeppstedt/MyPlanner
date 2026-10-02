using MyPlanner.Domain.Planners;

namespace MyPlanner.Application.Features.Planners.Commands.CreatePlanner
{
    public sealed record CreatePlannerResponse(PlannerId PlannerId, string Title);
}
