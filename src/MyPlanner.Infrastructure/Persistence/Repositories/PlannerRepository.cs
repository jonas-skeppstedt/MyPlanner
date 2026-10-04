using Microsoft.EntityFrameworkCore;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Shared;

namespace MyPlanner.Infrastructure.Persistence.Repositories
{
    internal class PlannerRepository : IPlannerRepository
    {
        private readonly ApplicationDbContext _context;

        public PlannerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Planner?> GetByIdAsync(PlannerId id, UserId userId, CancellationToken cancellationToken = default)
        {
            return await _context.Planners
                .Include(p => p.Todos)
                .Where(p => p.Id == id && p.OwnerId == userId)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public void Add(Planner planner)
        {
            _context.Planners.Add(planner);
        }

        public void Remove(Planner planner)
        {
            _context.Planners.Remove(planner);
        }
    }
}
