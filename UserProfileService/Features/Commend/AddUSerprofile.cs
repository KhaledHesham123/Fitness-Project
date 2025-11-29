using Azure.Core;
using MediatR;
using UserProfileService.Contract;
using UserProfileService.Entites;
using UserProfileService.Shared.Dto;
using UserProfileService.Shared.Response;

namespace UserProfileService.Features.Commend
{
    public record AddUSerprofile(Guid id, string FirstName, string LastName, string? ProfilePictureUrl, DateTime DateOfBirth
       ,Gender Gender, decimal Weight, decimal Height,
        FitnessGoal FitnessGoal) : IRequest<RequestResponse<USerprofileDTo>>;
    public class AddUserprofileHandler(IGenericRepository<UserProfile> _genericRepository) : IRequestHandler<AddUSerprofile, RequestResponse<USerprofileDTo>>
    {
        public async Task<RequestResponse<USerprofileDTo>> Handle(AddUSerprofile request, CancellationToken cancellationToken)
        {
            try
            {
                var model = new UserProfileService.Entites.UserProfile
                {
                     Id =  request.id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    ProfilePictureUrl = request.ProfilePictureUrl,
                    DateOfBirth = request.DateOfBirth,
                    Gender = request.Gender,
                    Weight = request.Weight,
                    Height = request.Height,
                    FitnessGoal = request.FitnessGoal 
                };
                _genericRepository.Add(model);
                await _genericRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // 💥 Unexpected error: handled by global middleware (returns 500)
                return Shared.Response.RequestResponse<USerprofileDTo>.Fail(ex.ToString(), 400);
            }
            var newmodel = new USerprofileDTo
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                ProfilePictureUrl = request.ProfilePictureUrl,
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                Weight = request.Weight,
                Height = request.Height,
                FitnessGoal = request.FitnessGoal,
            };

            return  RequestResponse<USerprofileDTo>.Success(newmodel, "plan add successfily");
        }
    }
}
