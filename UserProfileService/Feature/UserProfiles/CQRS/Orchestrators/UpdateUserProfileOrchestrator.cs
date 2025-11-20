using MediatR;
using UserProfileService.Feature.UserProfiles.CQRS.Commends;
using UserProfileService.Feature.UserProfiles.CQRS.Quries;
using UserProfileService.Shared.Entites;
using UserProfileService.Shared.Response;

namespace UserProfileService.Feature.UserProfiles.CQRS.Orchestrators
{
    public record UpdateUserProfileOrchestrator(Guid userid,IFormFile newImage):IRequest<RequestResponse<bool>>;


    public class UpdateUserProfileOrchestratorHandler : IRequestHandler<UpdateUserProfileOrchestrator, RequestResponse<bool>>
    {
        private readonly IMediator mediator;

        public UpdateUserProfileOrchestratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<RequestResponse<bool>> Handle(UpdateUserProfileOrchestrator request, CancellationToken cancellationToken)
        {
            if (request.userid==Guid.Empty)
            {
                return RequestResponse<bool>.Fail("there is no user with this id", 400);
            }

            var user = await mediator.Send(new GetUserByidQuery(request.userid));

            if (!user.IsSuccess)
                return RequestResponse<bool>.Fail("there is no user with this id", 400);

            var mappedUser = new UserProfile
            {
                DateOfBirth = user.Data.DateOfBirth,
                FirstName = user.Data.FirstName,
                FitnessGoal = user.Data.FitnessGoal,
                Gender = user.Data.Gender,
                Height = user.Data.Height,
                Id = user.Data.Id,
                LastName = user.Data.LastName,
                planid = user.Data.planid,
                ProfilePictureUrl = user.Data.ProfilePictureUrl,
                Weight = user.Data.Weight,
            };

            var result = await mediator.Send(new UpdateUserProfilePictureCommend(mappedUser, request.newImage));

            if (!result.IsSuccess)
            {
                return RequestResponse<bool>.Fail("some thing wnet wronge with updateing profile picture", 400);

            }

            return RequestResponse<bool>.Success(true);

        }
    }
}
