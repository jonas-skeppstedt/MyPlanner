using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Planners.ValueObjects;

namespace MyPlanner.Application.Features.Planners.Commands.ChangeTodoDescription
{
    internal sealed class ChangeTodoDescriptionCommandHandler
        : ICommandHandler<ChangeTodoDescriptionCommand, ChangeTodoDescriptionResponse>
    {
        private readonly IPlannerRepository _plannerRepository;
        private readonly IUserContext _userContext;

        public ChangeTodoDescriptionCommandHandler(IPlannerRepository plannerRepository, IUserContext userContext)
        {
            _plannerRepository = plannerRepository;
            _userContext = userContext;
        }

        public async Task<Result<ChangeTodoDescriptionResponse>> Handle(
            ChangeTodoDescriptionCommand command,
            CancellationToken cancellationToken)
        {
            var newTodoDescriptionResult = TodoDescription.Create(command.NewDescription);

            if (newTodoDescriptionResult.IsFailure)
            {
                return newTodoDescriptionResult.Error;
            }

            var planner = await _plannerRepository.GetByIdAsync(
                command.PlannerId,
                _userContext.UserId,
                cancellationToken);

            if (planner == null)
            {
                return PlannerErrors.NotFound;
            }

            var changeTodoDescriptionResult = planner.ChangeTodoDescription(
                command.TodoId,
                newTodoDescriptionResult.Value);

            if (changeTodoDescriptionResult.IsFailure)
            {
                return changeTodoDescriptionResult.Error;
            }

            if (changeTodoDescriptionResult.IsNoOp)
            {
                return Result<ChangeTodoDescriptionResponse>.NoOp();
            }

            var response = new ChangeTodoDescriptionResponse(
                command.TodoId,
                newTodoDescriptionResult.Value.Value);

            return Result<ChangeTodoDescriptionResponse>.Success(response);
        }
    }
}
