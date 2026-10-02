using MyPlanner.Domain.Planners;

namespace MyPlanner.Application.Features.Planners.Queries.Dtos
{
    public sealed record PlannerOverviewDto(PlannerId PlannerId, string Title);
}
