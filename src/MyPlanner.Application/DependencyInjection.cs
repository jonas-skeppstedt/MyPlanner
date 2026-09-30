using Microsoft.Extensions.DependencyInjection;
using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Contexts;

namespace MyPlanner.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });

            services.AddScoped<IUserContext, UserContext>();

            return services;
        }
    }
}
