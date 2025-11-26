using MediatR;
using Microsoft.Build.Framework;
using UserProfileService.Feature.UserProfiles.DTOs;
using UserProfileService.Shared.Entites;
using UserProfileService.Shared.GenericRepos;
using UserProfileService.Shared.Response;

namespace UserProfileService.Feature.UserProfiles.CQRS.Quries
{
    public record GetUsersbyplanid(IEnumerable<Guid> PLanIds) :IRequest<RequestResponse<IEnumerable<UserToReturnDto>>>;

    public class classGetUsersbyplanidHandler : IRequestHandler<GetUsersbyplanid, RequestResponse<IEnumerable<UserToReturnDto>>>
    {
        private readonly IGenericRepository<UserProfile> genericRepository;

        public classGetUsersbyplanidHandler(IGenericRepository<UserProfile> genericRepository)
        {
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<IEnumerable<UserToReturnDto>>> Handle(GetUsersbyplanid request, CancellationToken cancellationToken)
        {
            if (!request.PLanIds.Any())
                return RequestResponse<IEnumerable<UserToReturnDto>>.Fail("There is  no id",400);

            var users = await genericRepository.FindAsync(x => request.PLanIds.Contains(x.planid.Value));

            if (users == null || !users.Any())
                return RequestResponse<IEnumerable<UserToReturnDto>>.Fail("There are no users with this plan id", 404);

            var mappedUsers = users.Select(x => new UserToReturnDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DateOfBirth = x.DateOfBirth,
                Gender=x.Gender.ToString(),
                Height=x.Height,
                planid=x.planid,
                ProfilePictureUrl=x.ProfilePictureUrl,
                Weight=x.Weight,
               
            }).ToList();

            return RequestResponse<IEnumerable<UserToReturnDto>>
                .Success(mappedUsers, "Users retrieved successfully", 200);
        }
    }
}
