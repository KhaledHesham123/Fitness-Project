using System.Diagnostics.Eventing.Reader;
using System.Net.Http;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Extensions;
using UserProfileService.Contract;
using UserProfileService.Entites;
using UserProfileService.Features.Commend;
using UserProfileService.Shared.Dto;
using UserProfileService.Shared.Response;
namespace UserProfileService.Features.Orchestrators
{
    public class UpdateUserProfileQrccs : IUpdateUserProfileQrccs
    {
        private readonly IImageHelper _imageHelper;
        private readonly IMediator _mediator;
        private readonly IGenericRepository<UserProfile> _repository;
        private readonly HttpClient httpClient1;
        public UpdateUserProfileQrccs(IMediator mediator, IImageHelper imageTagHelper, IGenericRepository<UserProfile> repository,
            HttpClient httpClient)
        {
            _mediator = mediator;
            _imageHelper = imageTagHelper;
            httpClient1 = httpClient;
            _repository = repository;
        }

        public async Task<RequestResponse<USerprofileDTo>> UpdateUserprofileEditAsync(Guid Id, AddUserProfileRequest request)
        {
            if (Id == Guid.Empty)
                return RequestResponse<USerprofileDTo>.Fail("there is no user with this id", 400);
            var model = await _repository.GetByIdAsync(Id, u => u.ProgressHistory);
            if (model is null)
                return RequestResponse<USerprofileDTo>.Fail("not found", 400);

            var pictureUrl = model.ProfilePictureUrl;
            try
            {
                if (request.ProfilePictureUrl is not null && request.ProfilePictureUrl.Length > 0)
                {
                    if (!string.IsNullOrEmpty(model.ProfilePictureUrl))
                        _imageHelper.DeleteImage(model.ProfilePictureUrl, "Images");
                    pictureUrl = await _imageHelper.SaveImageAsync(request.ProfilePictureUrl, "Images");
                }
                model.ProfilePictureUrl = pictureUrl;
            }
            catch (Exception ex)
            {
                // Let the middleware handle it
                return RequestResponse<USerprofileDTo>.Fail(ex.Message, 400);
            }
            var updatemodel  = await  _mediator.Send(new UpdateUSerProfilePicturecs(model));
            return RequestResponse<USerprofileDTo>.Success(updatemodel.Data, "save the update",200);
        }

        public async Task<RequestResponse<bool>> UpdateUserprofilePassword(Guid id, ResetPassward request, HttpClient httpClient)
        {
            request.Userid = id;
            var response = await httpClient.PutAsJsonAsync("http://identity-service/api/ResetPassword", request);
            if (response.IsSuccessStatusCode)
                return RequestResponse<bool>.Success(true);
            else
                return RequestResponse<bool>.Fail("Fail", 400);


        }


    }
}

