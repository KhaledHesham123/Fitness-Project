using MediatR;
using UserProfileService.Shared.Entites;
using UserProfileService.Shared.GenericRepos;
using UserProfileService.Shared.Response;
using UserProfileService.Shared.Services.attachmentServices;

namespace UserProfileService.Feature.UserProfiles.CQRS.Commends
{
    public record UpdateUserProfilePictureCommend(UserProfile User,IFormFile file):IRequest<RequestResponse<bool>>;

    public class UpdateUserProfilePictureCommendHandler : IRequestHandler<UpdateUserProfilePictureCommend, RequestResponse<bool>>
    {
        private readonly IattachmentService attachmentService;
        private readonly IGenericRepository<UserProfile> genericRepository;

        public UpdateUserProfilePictureCommendHandler(IattachmentService attachmentService,IGenericRepository<UserProfile> genericRepository)
        {
            this.attachmentService = attachmentService;
            this.genericRepository = genericRepository;
        }
        public async Task<RequestResponse<bool>> Handle(UpdateUserProfilePictureCommend request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.User == null)
                {
                    return RequestResponse<bool>.Fail("there is no user", 400);
                }
                var newimage = attachmentService.UploadImage(request.file, "Images");

                if (newimage == null)
                    return RequestResponse<bool>.Fail("Invalid file type or size", 400);


                request.User.ProfilePictureUrl = newimage;

                genericRepository.SaveInclude(request.User);

                await genericRepository.SaveChanges();

                return RequestResponse<bool>.Success(true,"image Updated succesfuly");
            }
            catch (Exception)
            {
                return RequestResponse<bool>.Fail("some thinge went wronge ", 400);


            }






        }
    }
}
