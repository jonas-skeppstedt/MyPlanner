using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Application.Features.Planners.Queries.Dtos;

namespace MyPlanner.Application.Features.Planners.Queries.GetPlannerOverviews
{
    public sealed record GetPlannerOverviewsQuery : IQuery<IReadOnlyList<PlannerOverviewDto>>, IRequireAuthentication;
}
