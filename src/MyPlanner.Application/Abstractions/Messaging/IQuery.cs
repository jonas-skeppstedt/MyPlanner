using MediatR;
using MyPlanner.Domain.Common;

namespace MyPlanner.Application.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
