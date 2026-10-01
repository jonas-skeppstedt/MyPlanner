using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Domain.Common;

namespace MyPlanner.Infrastructure.Messaging
{
    /// <summary>
    /// Sends application commands and queries within an isolated DI scope.
    /// </summary>
    internal class ScopedSender : IScopedSender
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ICurrentUserService _currentUserService;

        public ScopedSender(IServiceScopeFactory scopeFactory, ICurrentUserService currentUserService)
        {
            _scopeFactory = scopeFactory;
            _currentUserService = currentUserService;
        }

        public Task<Result<T>> Query<T>(IQuery<T> query, CancellationToken cancellationToken = default)
            => Send(query, cancellationToken);

        public Task<Result> Command(ICommand command, CancellationToken cancellationToken = default)
            => Send(command, cancellationToken);

        public Task<Result<T>> Command<T>(ICommand<T> command, CancellationToken cancellationToken = default)
            => Send(command, cancellationToken);

        private async Task<TResponse> Send<TResponse>(
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default)
        {
            // Note:
            // Must fetch current UserId prior to creating the child scope
            // to avoid losing outer scope context
            var userId = await _currentUserService.GetCurrentUserIdAsync();

            await using var scope = _scopeFactory.CreateAsyncScope();

            var userContext = scope.ServiceProvider.GetRequiredService<IUserContext>();
            userContext.SetUserId(userId);

            var sender = scope.ServiceProvider.GetRequiredService<ISender>();
            return await sender.Send(request, cancellationToken);
        }
    }
}
