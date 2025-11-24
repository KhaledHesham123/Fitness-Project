using MediatR;
using Microsoft.Build.Framework;
using UserProfileService.Feature.UserProfiles.DTOs;
using UserProfileService.Shared.Entites;
using UserProfileService.Shared.GenericRepos;
using UserProfileService.Shared.Response;

namespace UserProfileService.Feature.UserProfiles.CQRS.Quries
{
    public record GetUsersbyplanid(IEnumerable<Guid> UserIds) :IRequest<RequestResponse<IEnumerable<UserToReturnDto>>>;

    public class classGetUsersbyplanidHandler : IRequestHandler<GetUsersbyplanid, RequestResponse<IEnumerable<UserToReturnDto>>>
    {
        private readonly IGenericRepository<UserProfile> genericRepository;

        public classGetUsersbyplanidHandler(IGenericRepository<UserProfile> genericRepository)
        {
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<IEnumerable<UserToReturnDto>>> Handle(GetUsersbyplanid request, CancellationToken cancellationToken)
        {
            if (!request.UserIds.Any())
                return RequestResponse<IEnumerable<UserToReturnDto>>.Fail("There is  no id",400);

            var users = await genericRepository.FindAsync(x => request.UserIds.Contains(x.Id));

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
                    Weight = user.Weight,
                    Height = user.Height,
                    planid = user.planid,
                });
            }

            return RequestResponse<IEnumerable<UserToReturnDto>>
                .Success(mappedUsers, "Users retrieved successfully", 200);
        }
    }
}
