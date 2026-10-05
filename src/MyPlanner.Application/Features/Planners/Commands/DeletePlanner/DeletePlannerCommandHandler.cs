using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Abstractions.Messaging;
using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners;

namespace MyPlanner.Application.Features.Planners.Commands.DeletePlanner
{
    internal sealed class DeletePlannerCommandHandler : ICommandHandler<DeletePlannerCommand>
    {
        private readonly IPlannerRepository _plannerRepository;
        private readonly IUserContext _userContext;

        public DeletePlannerCommandHandler(IPlannerRepository plannerRepository, IUserContext userContext)
        {
            _plannerRepository = plannerRepository;
            _userContext = userContext;
        }

        public Task<Result> Handle(DeletePlannerCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
