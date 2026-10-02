using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.ValueObjects;

namespace MyPlanner.Application.Features.Planners.Commands.CreatePlanner
{
    internal sealed class CreatePlannerCommandHandler : ICommandHandler<CreatePlannerCommand, CreatePlannerResponse>
    {
        private readonly IPlannerRepository _plannerRepository;
        private readonly IUserContext _userContext;

        public CreatePlannerCommandHandler(IPlannerRepository plannerRepository, IUserContext userContext)
        {
            _plannerRepository = plannerRepository;
            _userContext = userContext;
        }

        public async Task<Result<CreatePlannerResponse>> Handle(
            CreatePlannerCommand command,
            CancellationToken cancellationToken)
        {
            var plannerTitleResult = PlannerTitle.Create(command.Title);

            if (plannerTitleResult.IsFailure)
            {
                return plannerTitleResult.Error;
            }

            var plannerCreateResult = Planner.Create(
                PlannerId.New(),
                _userContext.UserId,
                plannerTitleResult.Value);

            if (plannerCreateResult.IsFailure)
            {
                return plannerCreateResult.Error;
            }

            var planner = plannerCreateResult.Value;

            _plannerRepository.Add(planner);

            var response = new CreatePlannerResponse(planner.Id, planner.Title.Value);

            return Result<CreatePlannerResponse>.Success(response);
        }
    }
}
