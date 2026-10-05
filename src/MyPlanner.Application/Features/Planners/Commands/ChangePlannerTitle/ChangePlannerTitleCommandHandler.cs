using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Planners.ValueObjects;

namespace MyPlanner.Application.Features.Planners.Commands.ChangePlannerTitle
{
    internal sealed class ChangePlannerTitleCommandHandler
        : ICommandHandler<ChangePlannerTitleCommand, ChangePlannerTitleResponse>
    {
        private readonly IPlannerRepository _plannerRepository;
        private readonly IUserContext _userContext;

        public ChangePlannerTitleCommandHandler(IPlannerRepository plannerRepository, IUserContext userContext)
        {
            _plannerRepository = plannerRepository;
            _userContext = userContext;
        }

        public async Task<Result<ChangePlannerTitleResponse>> Handle(
            ChangePlannerTitleCommand command,
            CancellationToken cancellationToken)
        {
            var newPlannerTitleResult = PlannerTitle.Create(command.NewTitle);

            if (newPlannerTitleResult.IsFailure)
            {
                return newPlannerTitleResult.Error;
            }

            var planner = await _plannerRepository.GetByIdAsync(
                command.PlannerId,
                _userContext.UserId,
                cancellationToken);

            if (planner == null)
            {
                return PlannerErrors.NotFound;
            }

            var changeTitleResult = planner.ChangeTitle(newPlannerTitleResult.Value);

            if (changeTitleResult.IsNoOp)
            {
                return Result<ChangePlannerTitleResponse>.NoOp();
            }

            var response = new ChangePlannerTitleResponse(planner.Title.Value);

            return Result<ChangePlannerTitleResponse>.Success(response);
        }
    }
}
