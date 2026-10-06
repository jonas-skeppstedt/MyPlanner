using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;

namespace MyPlanner.Application.Features.Planners.Commands.RemoveTodo
{
    internal sealed class RemoveTodoCommandHandler : ICommandHandler<RemoveTodoCommand>
    {
        private readonly IPlannerRepository _plannerRepository;
        private readonly IUserContext _userContext;

        public RemoveTodoCommandHandler(IPlannerRepository plannerRepository, IUserContext userContext)
        {
            _plannerRepository = plannerRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(RemoveTodoCommand command, CancellationToken cancellationToken)
        {
            var planner = await _plannerRepository.GetByIdAsync(
                command.PlannerId,
                _userContext.UserId,
                cancellationToken);

            if (planner == null)
            {
                return PlannerErrors.NotFound;
            }

            var removeTodoResult = planner.RemoveTodo(command.TodoId);

            if (removeTodoResult.IsFailure)
            {
                return removeTodoResult.Error;
            }

            return Result.Success();
        }
    }
}
