using MediatR;
using MyPlanner.Domain.Common;

namespace MyPlanner.Application.Abstractions.Messaging
{
    public interface IQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<TResponse>>
        where TRequest : IQuery<TResponse>
    {
    }
}
