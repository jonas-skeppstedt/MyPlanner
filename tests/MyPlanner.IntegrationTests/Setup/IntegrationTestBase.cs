using Microsoft.Extensions.DependencyInjection;
using MyPlanner.Infrastructure.Persistence;

namespace MyPlanner.IntegrationTests.Setup
{
    [Collection(nameof(IntegrationTestCollection))]
    public abstract class IntegrationTestBase : IAsyncLifetime
    {
        protected IServiceProvider Services { get; }

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
    }
}
