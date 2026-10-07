using MyPlanner.Domain.Planners;

namespace MyPlanner.Application.Features.Planners.Commands.ChangeTodoDescription
{
    public sealed record ChangeTodoDescriptionResponse(TodoId TodoId, string NewDescription);
}
