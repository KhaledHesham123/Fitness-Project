using MediatR;
using UserProfileService.Contract;
using UserProfileService.Entites;
using UserProfileService.Shared.Dto;
using UserProfileService.Shared.Response;

namespace UserProfileService.Features.Commend
{
    public record UpdateUSerProfilePicturecs(UserProfile User) :IRequest<RequestResponse<USerprofileDTo>>;

    public class UpdateUSerProfilePicturecsHandler(IGenericRepository<UserProfile> repository) : IRequestHandler<UpdateUSerProfilePicturecs, RequestResponse<USerprofileDTo>>
    {
    public async Task<RequestResponse<USerprofileDTo>> Handle(UpdateUSerProfilePicturecs request, CancellationToken cancellationToken)
        {
            try {
               
                  var user = new UserProfile() 
                  {
                    FirstName =request.User.FirstName,
                    LastName = request.User.LastName,
                    DateOfBirth = request.User.DateOfBirth,
                    Gender = request.User.Gender,
                    Weight  = request.User.Weight,
                    Height  = request.User.Height,
                    ProfilePictureUrl   = request.User.ProfilePictureUrl,
                    FitnessGoal = request.User.FitnessGoal
                  };

                repository.Update(user);
               await repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return RequestResponse<USerprofileDTo>.Fail("some thinge went wronge ", 400);
            }
            var model= new USerprofileDTo
            {
                FirstName = request.User.FirstName,
                LastName = request.User.LastName,
                DateOfBirth = request.User.DateOfBirth,
                Gender = request.User.Gender,
                Weight = request.User.Weight,
                Height = request.User.Height,
                ProfilePictureUrl = request.User.ProfilePictureUrl,
                FitnessGoal = request.User.FitnessGoal
            };

            return RequestResponse<USerprofileDTo>.Success(model, "succesful", 200);
        }
    }
}
