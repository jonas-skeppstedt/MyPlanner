using MyPlanner.Domain.Common;

namespace MyPlanner.Application.Abstractions.Messaging
{
    /// <summary>
    /// Sends application commands and queries within an isolated DI scope.
    /// </summary>
    public interface IScopedSender
    {
        Task<Result<T>> Query<T>(IQuery<T> query, CancellationToken cancellationToken = default);
        Task<Result> Command(ICommand command, CancellationToken cancellationToken = default);
        Task<Result<T>> Command<T>(ICommand<T> command, CancellationToken cancellationToken = default);
    }
}
