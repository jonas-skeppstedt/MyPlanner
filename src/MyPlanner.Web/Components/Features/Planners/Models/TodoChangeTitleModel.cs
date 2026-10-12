using MyPlanner.Domain.Planners;
using System.ComponentModel.DataAnnotations;

namespace MyPlanner.Web.Components.Features.Planners.Models
{
    public class TodoChangeTitleModel
    {
        public PlannerId PlannerId { get; set; }
        public TodoId TodoId { get; set; }

        [Required]
        [StringLength(25)]
        public string NewTitle { get; set; } = default!;
    }
}
