using Moq;
using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Features.Planners.Commands.CreatePlanner;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Shared;

namespace MyPlanner.Application.UnitTests.Features.Planners.Commands.CreatePlanner
{
    public class CreatePlannerCommandHandlerUnitTests
    {
        private readonly CreatePlannerCommandHandler _sut;
        private readonly Mock<IPlannerRepository> _plannerRepositoryMock = new();
        private readonly Mock<IUserContext> _userContextMock = new();

        public CreatePlannerCommandHandlerUnitTests()
        {
            _sut = new(_plannerRepositoryMock.Object, _userContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WhenCommandIsInvalid()
        {
            // Arrange
            var invalidCommand = new CreatePlannerCommand(string.Empty);
            var expectedError = PlannerTitleErrors.TitleRequired;

            // Act
            var result = await _sut.Handle(invalidCommand, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WhenCommandIsValid()
        {
            // Arrange
            var expectedTitle = "Valid title";
            var expectedUserId = new UserId(Guid.NewGuid());
            var validCommand = new CreatePlannerCommand(expectedTitle);

            Planner? addedPlanner = null;

            _userContextMock
                .Setup(x => x.UserId)
                .Returns(expectedUserId);

            _plannerRepositoryMock
                .Setup(x => x.Add(It.IsAny<Planner>()))
                .Callback<Planner>(p => addedPlanner = p);

            // Act
            var result = await _sut.Handle(validCommand, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedTitle, result.Value.Title);
            Assert.NotEqual(default, result.Value.PlannerId);

            Assert.NotNull(addedPlanner);
            Assert.Equal(result.Value.PlannerId, addedPlanner.Id);
            Assert.Equal(expectedTitle, addedPlanner.Title.ToString());
            Assert.Equal(expectedUserId, addedPlanner.OwnerId);

            // Verify
            _plannerRepositoryMock
                .Verify(x => x.Add(It.IsAny<Planner>()), Times.Once);
        }
    }
}
