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
    }
}
