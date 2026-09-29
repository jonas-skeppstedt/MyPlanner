using MyPlanner.Domain.Shared;

namespace MyPlanner.Domain.Planners
{
    public interface IPlannerRepository
    {
        Task<IReadOnlyList<Planner>> GetAllAsync(UserId userId, CancellationToken cancellationToken = default);
        Task<Planner?> GetByIdAsync(PlannerId id, UserId userId, CancellationToken cancellationToken = default);
        void Add(Planner planner);
        void Remove(Planner planner);
    }
}
