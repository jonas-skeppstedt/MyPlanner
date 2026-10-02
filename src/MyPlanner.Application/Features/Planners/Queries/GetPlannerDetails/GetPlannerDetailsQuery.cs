using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Application.Features.Planners.Queries.Dtos;
using MyPlanner.Domain.Planners;

namespace MyPlanner.Application.Features.Planners.Queries.GetPlannerDetails
{
    public sealed record GetPlannerDetailsQuery(PlannerId PlannerId)
        : IQuery<PlannerDetailsDto>, IRequireAuthentication;
}
