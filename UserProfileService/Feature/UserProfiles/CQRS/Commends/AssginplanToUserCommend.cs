using MediatR;
using Microsoft.Build.Framework;
using UserProfileService.Shared.Entites;
using UserProfileService.Shared.GenericRepos;
using UserProfileService.Shared.Response;

namespace UserProfileService.Feature.UserProfiles.CQRS.Commends
{
    public record AssginplanToUserCommend(UserProfile UserProfile,Guid planid) :IRequest<RequestResponse<UserProfile>>;

    public class AssginplanToUserCommendHandler : IRequestHandler<AssginplanToUserCommend, RequestResponse<UserProfile>>
    {
        private readonly IGenericRepository<UserProfile> _genericRepository;

        public AssginplanToUserCommendHandler(IGenericRepository<UserProfile> genericRepository)
        {
            this._genericRepository = genericRepository;
        }
        public async Task<RequestResponse<UserProfile>> Handle(AssginplanToUserCommend request, CancellationToken cancellationToken)
        {
            var user=request.UserProfile;

            if (request.UserProfile==null)
            {

                return RequestResponse<UserProfile>.Fail("error there is no user", 404);
            }

            user.planid= request.planid;

            try
            {
                _genericRepository.SaveInclude(user);
                await _genericRepository.SaveChanges();

                return RequestResponse<UserProfile>.Success(user,"plan add successfily");

            }
            catch (Exception ex)
            {

                return RequestResponse<UserProfile>.Fail(ex.ToString(), 400);
            }
            
        }
    }
}
