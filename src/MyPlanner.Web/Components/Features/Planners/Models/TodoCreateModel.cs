using MyPlanner.Domain.Planners;
using System.ComponentModel.DataAnnotations;

namespace MyPlanner.Web.Components.Features.Planners.Models
{
    public class TodoCreateModel
    {
        [Required]
        public PlannerId PlannerId { get; set; }

        [Required]
        public string Title { get; set; } = default!;
    }
}
