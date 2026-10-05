using Moq;
using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Features.Planners.Commands.DeletePlanner;
using MyPlanner.Domain.Planners;

namespace MyPlanner.Application.UnitTests.Features.Planners.Commands.DeletePlanner
{
    public class DeletePlannerCommandHandlerUnitTests
    {
        private readonly DeletePlannerCommandHandler _sut;
        private readonly Mock<IPlannerRepository> _plannerRepositoryMock = new();
        private readonly Mock<IUserContext> _userContextMock = new();

        public DeletePlannerCommandHandlerUnitTests()
        {
            _sut = new(_plannerRepositoryMock.Object, _userContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WhenPlannerWasNotFound()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WhenUserIsNotOwner()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WhenDeleteIsSuccessful()
        {
            throw new NotImplementedException();
        }
    }
}
