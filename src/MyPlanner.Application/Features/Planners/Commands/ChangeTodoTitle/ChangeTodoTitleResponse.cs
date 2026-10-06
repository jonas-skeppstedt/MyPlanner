using MyPlanner.Domain.Planners;

namespace MyPlanner.Application.Features.Planners.Commands.ChangeTodoTitle
{
    public sealed record ChangeTodoTitleResponse(TodoId TodoId, string NewTitle);
}
