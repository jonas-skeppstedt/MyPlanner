using MediatR;
using MyPlanner.Application.Errors;
using MyPlanner.Domain.Common;

namespace MyPlanner.Application.Behaviors
{
    internal sealed class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
        where TResponse : Result, IFailureBuildable<TResponse>
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch
            {
                return TResponse.Failure(SystemErrors.UnexpectedError);
            }
        }
    }
}
