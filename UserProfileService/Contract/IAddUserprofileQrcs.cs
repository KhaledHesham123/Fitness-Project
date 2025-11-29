using MediatR;
using UserProfileService.Shared.Dto;
using UserProfileService.Shared.Response;
namespace UserProfileService.Contract
{
    public interface IAddUserprofileQrcs
    {
        public Task<RequestResponse<USerprofileDTo>> AddUserProfileAsync(Guid id, AddUserProfileRequest request);
    }
}
