using MediatR;
using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Errors;
using MyPlanner.Domain.Common;

namespace MyPlanner.Application.Behaviors
{
    internal sealed class AuthenticationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequireAuthentication
        where TResponse : Result, IFailureBuildable<TResponse>
    {
        private readonly IUserContext _userContext;

        public AuthenticationBehavior(IUserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var userId = _userContext.UserId;

            if (userId == default)
            {
                return TResponse.Failure(AuthenticationErrors.UserNotAuthenticated);
            }

            return await next();
        }
    }
}
