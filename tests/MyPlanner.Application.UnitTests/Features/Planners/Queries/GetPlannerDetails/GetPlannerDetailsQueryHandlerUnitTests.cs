using Moq;
using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Features.Planners.Queries;
using MyPlanner.Application.Features.Planners.Queries.Dtos;
using MyPlanner.Application.Features.Planners.Queries.GetPlannerDetails;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Shared;

namespace MyPlanner.Application.UnitTests.Features.Planners.Queries.GetPlannerDetails
{
    public class GetPlannerDetailsQueryHandlerUnitTests
    {
        private readonly GetPlannerDetailsQueryHandler _sut;
        private readonly Mock<IPlannerQueries> _plannerQueriesMock = new();
        private readonly Mock<IUserContext> _userContextMock = new();

        public GetPlannerDetailsQueryHandlerUnitTests()
        {
            _sut = new(_plannerQueriesMock.Object, _userContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WhenPlannerWithProvidedIdIsNotFound()
        {
            // Arrange
            var query = new GetPlannerDetailsQuery(PlannerId.New());

            _plannerQueriesMock
                .Setup(x => x.GetPlannerDetailsAsync(
                    It.IsAny<PlannerId>(),
                    It.IsAny<UserId>(),
                    CancellationToken.None))
                .ReturnsAsync((PlannerDetailsDto?)null);

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PlannerErrors.NotFound, result.Error);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WhenPlannerWithProvidedIdExists()
        {
            // Arrange
            var plannerId = PlannerId.New();
            var userId = new UserId(Guid.NewGuid());
            var expectedDto = new PlannerDetailsDto(plannerId, userId, "Valid title", []);
            var query = new GetPlannerDetailsQuery(plannerId);

            _userContextMock
                .Setup(x => x.UserId)
                .Returns(userId);

            _plannerQueriesMock
                .Setup(x => x.GetPlannerDetailsAsync(plannerId, userId, CancellationToken.None))
                .ReturnsAsync(expectedDto);

            // Act
            var result = await _sut.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedDto, result.Value);
        }
    }
}
