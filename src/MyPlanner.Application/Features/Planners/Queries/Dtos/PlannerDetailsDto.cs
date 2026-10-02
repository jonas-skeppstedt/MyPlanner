using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Shared;

namespace MyPlanner.Application.Features.Planners.Queries.Dtos
{
    public sealed record PlannerDetailsDto(
        PlannerId PlannerId,
        UserId OwnerId,
        string Title,
        IReadOnlyList<TodoDetailsDto> Todos);
}
