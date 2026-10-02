using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Application.Features.Planners.Queries.Dtos;
using MyPlanner.Domain.Common;

namespace MyPlanner.Application.Features.Planners.Queries.GetPlannerOverviews
{
    internal sealed class GetPlannerOverviewsQueryHandler
        : IQueryHandler<GetPlannerOverviewsQuery, IReadOnlyList<PlannerOverviewDto>>
    {
        private readonly IPlannerQueries _plannerQueries;
        private readonly IUserContext _userContext;

        public GetPlannerOverviewsQueryHandler(IPlannerQueries plannerQueries, IUserContext userContext)
        {
            _plannerQueries = plannerQueries;
            _userContext = userContext;
        }

        public async Task<Result<IReadOnlyList<PlannerOverviewDto>>> Handle(
            GetPlannerOverviewsQuery request,
            CancellationToken cancellationToken)
        {
            var dtos = await _plannerQueries.GetPlannerOverviewsAsync(_userContext.UserId, cancellationToken);

            return Result<IReadOnlyList<PlannerOverviewDto>>.Success(dtos);
        }
    }
}
