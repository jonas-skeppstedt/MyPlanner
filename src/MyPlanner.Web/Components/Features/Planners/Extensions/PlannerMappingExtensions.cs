using MyPlanner.Application.Features.Planners.Queries.Dtos;
using MyPlanner.Web.Components.Features.Planners.Models;

namespace MyPlanner.Web.Components.Features.Planners.Extensions
{
    public static class PlannerMappingExtensions
    {
        public static PlannerOverviewModel ToUiModel(this PlannerOverviewDto dto)
        {
            return new(dto.PlannerId, dto.Title);
        }

        public static PlannerDetailsModel ToUiModel(this PlannerDetailsDto dto)
        {
            return new(
                dto.PlannerId,
                dto.OwnerId,
                dto.Title,
                dto.Todos.Select(t => t.ToUiModel()).ToList());
        }

        public static TodoDetailsModel ToUiModel(this TodoDetailsDto dto)
        {
            return new(dto.TodoId, dto.Title, dto.Description);
        }
    }
}
