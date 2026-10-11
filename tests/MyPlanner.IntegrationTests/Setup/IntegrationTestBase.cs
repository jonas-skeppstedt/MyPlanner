using Microsoft.Extensions.DependencyInjection;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Domain.Planners;
using MyPlanner.Infrastructure.Persistence;

namespace MyPlanner.IntegrationTests.Setup
{
    [Collection(nameof(IntegrationTestCollection))]
    public abstract class IntegrationTestBase : IAsyncLifetime
    {
        protected IServiceProvider Services { get; }
        protected IScopedSender Sender => Services.GetRequiredService<IScopedSender>();

        protected IntegrationTestBase(IntegrationTestFixture integrationTestFixture)
        {
            Services = integrationTestFixture.Services;
        }

        public async Task InitializeAsync()
        {
            await using var scope = Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        protected async Task InsertPlannerAsync(Planner planner)
        {
            await using var scope = Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Add(planner);
            await dbContext.SaveChangesAsync();
        }

        protected async Task InsertPlannersRangeAsync(params Planner[] planners)
        {
            await using var scope = Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.AddRange(planners);
            await dbContext.SaveChangesAsync();
        }
    }
}
