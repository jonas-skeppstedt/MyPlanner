using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Planners.ValueObjects;

namespace MyPlanner.Application.Features.Planners.Commands.AddTodo
{
    internal sealed class AddTodoCommandHandler : ICommandHandler<AddTodoCommand, AddTodoResponse>
    {
        private readonly IPlannerRepository _plannerRepository;
        private readonly IUserContext _userContext;

        public AddTodoCommandHandler(IPlannerRepository plannerRepository, IUserContext userContext)
        {
            _plannerRepository = plannerRepository;
            _userContext = userContext;
        }

        public async Task<Result<AddTodoResponse>> Handle(AddTodoCommand command, CancellationToken cancellationToken)
        {
            var todoTitleResult = TodoTitle.Create(command.Title);

            if (todoTitleResult.IsFailure)
            {
                return todoTitleResult.Error;
            }

            var todoCreateResult = Todo.Create(TodoId.New(), todoTitleResult.Value);

            if (todoCreateResult.IsFailure)
            {
                return todoCreateResult.Error;
            }

            var planner = await _plannerRepository.GetByIdAsync(
                command.PlannerId,
                _userContext.UserId,
                cancellationToken);

            if (planner == null)
            {
                return PlannerErrors.NotFound;
            }

            var todo = todoCreateResult.Value;

            var addTodoResult = planner.AddTodo(todo);

            if (addTodoResult.IsFailure)
            {
                return addTodoResult.Error;
            }

            var response = new AddTodoResponse(
                todo.Id,
                todo.Title.Value,
                todo.Description.Value);

            return Result<AddTodoResponse>.Success(response);
        }
    }
}
