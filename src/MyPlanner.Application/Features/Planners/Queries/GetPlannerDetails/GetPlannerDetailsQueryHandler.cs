using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Application.Features.Planners.Queries.Dtos;
using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners.Errors;

namespace MyPlanner.Application.Features.Planners.Queries.GetPlannerDetails
{
    internal sealed class GetPlannerDetailsQueryHandler
        : IQueryHandler<GetPlannerDetailsQuery, PlannerDetailsDto>
    {
        private readonly IPlannerQueries _plannerQueries;
        private readonly IUserContext _userContext;

        public GetPlannerDetailsQueryHandler(IPlannerQueries plannerQueries, IUserContext userContext)
        {
            _plannerQueries = plannerQueries;
            _userContext = userContext;
        }

        public async Task<Result<PlannerDetailsDto>> Handle(
            GetPlannerDetailsQuery query,
            CancellationToken cancellationToken)
        {
            var dto = await _plannerQueries.GetPlannerDetailsAsync(
                query.PlannerId,
                _userContext.UserId,
                cancellationToken);

            if (dto == null)
            {
                return PlannerErrors.NotFound;
            }

            return Result<PlannerDetailsDto>.Success(dto);
        }
    }
}
