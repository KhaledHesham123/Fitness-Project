using MediatR;
using Microsoft.Build.Framework;
using UserProfileService.Shared.Entites;
using UserProfileService.Shared.GenericRepos;
using UserProfileService.Shared.Response;

namespace UserProfileService.Feature.UserProfiles.CQRS.Commends
{
    public record AssginplanToUserCommend(IEnumerable<UserProfile> Users,Guid planid) :IRequest<RequestResponse<bool>>;

    public class AssginplanToUserCommendHandler : IRequestHandler<AssginplanToUserCommend, RequestResponse<bool>>
    {
        private readonly IGenericRepository<UserProfile> _genericRepository;

        public AssginplanToUserCommendHandler(IGenericRepository<UserProfile> genericRepository)
        {
            this._genericRepository = genericRepository;
        }
        public async Task<RequestResponse<bool>> Handle(AssginplanToUserCommend request, CancellationToken cancellationToken)
        {

            if (!request.Users.Any())
            {
                return RequestResponse<bool>.Fail("Error: there are no users", 404);
            }

            try
            {
                foreach (var user in request.Users)
                {
                    user.planid = request.planid;
                    _genericRepository.SaveInclude(user);
                }

                await _genericRepository.SaveChanges();

                return RequestResponse<bool>.Success(true, "Plan added successfully to all users");
            }
            catch (Exception ex)
            {
                return RequestResponse<bool>.Fail(ex.ToString(), 400);
            }

        }
    }
}
