using MediatR;
using Microsoft.Build.Framework;
using UserProfileService.Feature.UserProfiles.DTOs;
using UserProfileService.Shared.Entites;
using UserProfileService.Shared.GenericRepos;
using UserProfileService.Shared.Response;

namespace UserProfileService.Feature.UserProfiles.CQRS.Quries
{
    public record GetUsersbyplanid(Guid id):IRequest<RequestResponse<IEnumerable<UserToReturnDto>>>;

    public class classGetUsersbyplanidHandler : IRequestHandler<GetUsersbyplanid, RequestResponse<IEnumerable<UserToReturnDto>>>
    {
        private readonly IGenericRepository<UserProfile> genericRepository;

        public classGetUsersbyplanidHandler(IGenericRepository<UserProfile> genericRepository)
        {
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<IEnumerable<UserToReturnDto>>> Handle(GetUsersbyplanid request, CancellationToken cancellationToken)
        {
            if (request.id == Guid.Empty)
                return RequestResponse<IEnumerable<UserToReturnDto>>.Fail("There is  no id",400);

            var users = await genericRepository.FindAsync(x => x.planid == request.id);

            if (users == null || !users.Any())
                return RequestResponse<IEnumerable<UserToReturnDto>>.Fail("There are no users with this plan id", 404);

            var mappedUsers = new List<UserToReturnDto>();

            foreach (var user in users)
            {
                mappedUsers.Add(new UserToReturnDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    Weight = user.Weight,
                    Height = user.Height,
                    FitnessGoal = user.FitnessGoal,
                    planid = user.planid,
                });
            }

            return RequestResponse<IEnumerable<UserToReturnDto>>
                .Success(mappedUsers, "Users retrieved successfully", 200);
        }
    }
}
