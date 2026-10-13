using MyPlanner.Domain.Planners;
using System.ComponentModel.DataAnnotations;

namespace MyPlanner.Web.Components.Features.Planners.Models
{
    public class TodoChangeDescriptionModel
    {
        public PlannerId PlannerId { get; set; }
        public TodoId TodoId { get; set; }

        [StringLength(200)]
        public string NewDescription { get; set; } = default!;
    }
}
