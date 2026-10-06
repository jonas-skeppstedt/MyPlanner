using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;

namespace MyPlanner.Application.Features.Planners.Commands.DeletePlanner
{
    internal sealed class DeletePlannerCommandHandler : ICommandHandler<DeletePlannerCommand>
    {
        private readonly IPlannerRepository _plannerRepository;
        private readonly IUserContext _userContext;

        public DeletePlannerCommandHandler(IPlannerRepository plannerRepository, IUserContext userContext)
        {
            _plannerRepository = plannerRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(DeletePlannerCommand command, CancellationToken cancellationToken)
        {
            var planner = await _plannerRepository.GetByIdAsync(
                command.PlannerId,
                _userContext.UserId,
                cancellationToken);

            if (planner == null)
            {
                return PlannerErrors.NotFound;
            }

            var removePlannerResult = planner.Delete(_userContext.UserId);

            if (removePlannerResult.IsFailure)
            {
                return removePlannerResult.Error;
            }

            _plannerRepository.Remove(planner);

            return Result.Success();
        }
    }
}
