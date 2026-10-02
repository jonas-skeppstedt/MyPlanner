using MyPlanner.Application.Features.Planners.Queries.Dtos;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Shared;

namespace MyPlanner.Application.Features.Planners.Queries
{
    public interface IPlannerQueries
    {
        Task<IReadOnlyList<PlannerOverviewDto>> GetPlannerOverviewsAsync(UserId userId, CancellationToken cancellationToken = default);
        Task<PlannerDetailsDto?> GetPlannerDetailsAsync(PlannerId id, UserId userId, CancellationToken cancellationToken = default);
    }
}
