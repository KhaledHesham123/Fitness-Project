using MediatR;
using Microsoft.AspNetCore.Identity;
using UserProfileService.Contract;
using UserProfileService.Features.Commend;
using UserProfileService.Shared.Dto;
using UserProfileService.Shared.Response;


namespace UserProfileService.Features.Orchestrators
{
    public class AddUserprofileQrcs: IAddUserprofileQrcs
    {
        private readonly IImageHelper _imageHelper;
        private readonly IMediator _mediator;
        public AddUserprofileQrcs(IImageHelper helperImage, IMediator mediator)
        {
            _imageHelper = helperImage;
            _mediator = mediator;
        }
        public async Task<RequestResponse<USerprofileDTo>> AddUserProfileAsync(Guid id, AddUserProfileRequest request)
        {



            if (request.ProfilePictureUrl == null)
                return RequestResponse<USerprofileDTo>.Fail("Picture is null", 400);
            var picture = await _imageHelper.SaveImageAsync(request.ProfilePictureUrl, "Images");

            var model = await _mediator.Send(new AddUSerprofile(
                id,
                request.FirstName,
                request.LastName,
                picture,
                request.DateOfBirth,
                request.Gender, 
                request.Weight,
                request.Height,
                request.FitnessGoal
             ));
            return model;
        }
    }
}
