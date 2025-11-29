

using UserProfileService.Entites;
using UserProfileService.Shared.Dto;
using UserProfileService.Shared.Response;

namespace UserProfileService.Contract
{
    public interface IUpdateUserProfileQrccs
    {

        public Task<RequestResponse<USerprofileDTo>> UpdateUserprofileEditAsync(Guid Id, AddUserProfileRequest request);

        public Task<RequestResponse<bool>> UpdateUserprofilePassword(Guid id, ResetPassward request, HttpClient httpClient);


    }
}
