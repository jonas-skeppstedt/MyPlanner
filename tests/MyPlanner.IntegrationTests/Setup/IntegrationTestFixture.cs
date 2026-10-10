using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyPlanner.Application;
using MyPlanner.Application.Abstractions;
using MyPlanner.Infrastructure;
using Testcontainers.PostgreSql;

namespace MyPlanner.IntegrationTests.Setup
{
    public class IntegrationTestFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:18-alpine")
            .WithDatabase("testdb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        public IServiceProvider Services { get; private set; } = null!;

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "ConnectionStrings:DefaultConnection", _dbContainer.GetConnectionString() }
                })
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddApplication();
            services.AddInfrastructure(configuration);

            services.AddScoped<ICurrentUserService, FakeCurrentUserService>();

            Services = services.BuildServiceProvider();
        }

        public async Task DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
        }
    }
}
