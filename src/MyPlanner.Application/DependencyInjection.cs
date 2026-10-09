using Microsoft.Extensions.DependencyInjection;
using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Behaviors;
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

                cfg.AddOpenBehavior(typeof(ExceptionHandlingBehavior<,>));
                cfg.AddOpenBehavior(typeof(AuthenticationBehavior<,>));
                cfg.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
            });

            services.AddScoped<IUserContext, UserContext>();

            return services;
        }
    }
}
