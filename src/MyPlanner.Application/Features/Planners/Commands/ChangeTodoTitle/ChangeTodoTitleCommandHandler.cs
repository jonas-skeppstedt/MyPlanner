using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Planners.ValueObjects;

namespace MyPlanner.Application.Features.Planners.Commands.ChangeTodoTitle
{
    internal sealed class ChangeTodoTitleCommandHandler
        : ICommandHandler<ChangeTodoTitleCommand, ChangeTodoTitleResponse>
    {
        private readonly IPlannerRepository _plannerRepository;
        private readonly IUserContext _userContext;

        public ChangeTodoTitleCommandHandler(IPlannerRepository plannerRepository, IUserContext userContext)
        {
            _plannerRepository = plannerRepository;
            _userContext = userContext;
        }

        public async Task<Result<ChangeTodoTitleResponse>> Handle(
            ChangeTodoTitleCommand command,
            CancellationToken cancellationToken)
        {
            var newTodoTitleResult = TodoTitle.Create(command.NewTitle);

            if (newTodoTitleResult.IsFailure)
            {
                return newTodoTitleResult.Error;
            }

            var planner = await _plannerRepository.GetByIdAsync(
                command.PlannerId,
                _userContext.UserId,
                cancellationToken);

            if (planner == null)
            {
                return PlannerErrors.NotFound;
            }

            var changeTodoTitleResult = planner.ChangeTodoTitle(command.TodoId, newTodoTitleResult.Value);

            if (changeTodoTitleResult.IsFailure)
            {
                return changeTodoTitleResult.Error;
            }

            if (changeTodoTitleResult.IsNoOp)
            {
                return Result<ChangeTodoTitleResponse>.NoOp();
            }

            var response = new ChangeTodoTitleResponse(command.TodoId, newTodoTitleResult.Value.Value);

            return Result<ChangeTodoTitleResponse>.Success(response);
        }
    }
}
