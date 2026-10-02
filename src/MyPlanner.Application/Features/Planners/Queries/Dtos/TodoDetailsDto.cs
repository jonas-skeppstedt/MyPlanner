using MyPlanner.Domain.Planners;

namespace MyPlanner.Application.Features.Planners.Queries.Dtos
{
    public sealed record TodoDetailsDto(
        TodoId TodoId,
        string Title,
        string Description);
}
