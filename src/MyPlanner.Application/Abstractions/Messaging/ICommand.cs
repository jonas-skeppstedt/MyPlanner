using MediatR;
using MyPlanner.Domain.Common;

namespace MyPlanner.Application.Abstractions.Messaging
{
    public interface ICommandBase
    {
    }

    public interface ICommand : IRequest<Result>, ICommandBase
    {
    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, ICommandBase
    {
    }
}
