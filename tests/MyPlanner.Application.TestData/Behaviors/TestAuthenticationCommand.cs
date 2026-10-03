using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;

namespace MyPlanner.Application.TestData.Behaviors
{
    public sealed record TestAuthenticationCommand : ICommand, IRequireAuthentication;
}
