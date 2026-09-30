
using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;

namespace MyPlanner.Application.UnitTests.TestData
{
    public sealed record TestAuthenticationCommand : ICommand, IRequireAuthentication;
}
