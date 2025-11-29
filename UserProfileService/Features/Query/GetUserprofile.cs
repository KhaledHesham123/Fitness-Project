using MediatR;
using UserProfileService.Contract;
using UserProfileService.Entites;
using UserProfileService.Shared.Dto;
using UserProfileService.Shared.Response;

namespace UserProfileService.Features.Query
{
    public record GetUserprofile(Guid id) : IRequest<RequestResponse<USerprofileDTo>>;

    public class GetUserProfile(IGenericRepository<UserProfile> genericRepository) : IRequestHandler<GetUserprofile, RequestResponse<USerprofileDTo>>
    {
        public async Task<RequestResponse<USerprofileDTo>> Handle(GetUserprofile request, CancellationToken cancellationToken)
        {
            if (request.id == Guid.Empty)
                return RequestResponse<USerprofileDTo>.Fail("ID can not be ull", 400);
          
             var user  = await genericRepository.GetByIdAsync(request.id, p=>p.ProgressHistory );

            if (user == null)
                return RequestResponse<USerprofileDTo>.Fail("No User have this id", 400);

            var MappedUser = new USerprofileDTo
            {
                
                DateOfBirth = user.DateOfBirth,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FitnessGoal = user.FitnessGoal,
                Gender = user.Gender,
                Height = user.Height,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Weight = user.Weight,
                ProgressHistory=user.ProgressHistory.ToList(),
            };

            return RequestResponse<USerprofileDTo>.Success(MappedUser, "Users retrieved successfully", 200);
        }
    }
}
