using MyPlanner.Domain.Planners;

namespace MyPlanner.Application.Features.Planners.Commands.ChangePlannerTitle
{
    public sealed record ChangePlannerTitleResponse(PlannerId PlannerId, string NewTitle);
}
