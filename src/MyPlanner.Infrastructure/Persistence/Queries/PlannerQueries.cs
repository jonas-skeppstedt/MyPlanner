using Microsoft.EntityFrameworkCore;
using MyPlanner.Application.Features.Planners.Queries;
using MyPlanner.Application.Features.Planners.Queries.Dtos;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Shared;

namespace MyPlanner.Infrastructure.Persistence.Queries
{
    internal class PlannerQueries : IPlannerQueries
    {
        private readonly ApplicationDbContext _context;

        public PlannerQueries(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<PlannerOverviewDto>> GetPlannerOverviewsAsync(
           UserId userId,
           CancellationToken cancellationToken = default)
        {
            return await _context.Planners
                .AsNoTracking()
                .Where(p => p.OwnerId == userId)
                .Select(p => new PlannerOverviewDto(
                    p.Id,
                    p.Title.Value))
                .ToListAsync(cancellationToken);
        }

        public async Task<PlannerDetailsDto?> GetPlannerDetailsAsync(
            PlannerId id,
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Planners
                .AsNoTracking()
                .Where(p => p.Id == id && p.OwnerId == userId)
                .Select(p => new PlannerDetailsDto(
                    p.Id,
                    p.OwnerId,
                    p.Title.Value,
                    p.Todos.Select(t => new TodoDetailsDto(
                        t.Id,
                        t.Title.Value,
                        t.Description.Value))
                    .ToList()))
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
