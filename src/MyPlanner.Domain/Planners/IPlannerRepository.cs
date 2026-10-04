using MyPlanner.Domain.Shared;

namespace MyPlanner.Domain.Planners
{
    public interface IPlannerRepository
    {
        Task<Planner?> GetByIdAsync(PlannerId id, UserId userId, CancellationToken cancellationToken = default);
        void Add(Planner planner);
        void Remove(Planner planner);
    }
}
