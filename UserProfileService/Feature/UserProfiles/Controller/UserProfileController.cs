using Azure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserProfileService.Feature.UserProfiles.CQRS.Commends;
using UserProfileService.Feature.UserProfiles.CQRS.Orchestrators;
using UserProfileService.Feature.UserProfiles.CQRS.Quries;
using UserProfileService.Feature.UserProfiles.DTOs;
using UserProfileService.Shared.Response;

namespace UserProfileService.Feature.UserProfiles.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        public UserProfileController(IMediator mediator)
        {
            Mediator = mediator;
        }

        public IMediator Mediator { get; }
        [HttpGet("GetUsersbyplanid")]

        public async Task<EndpointResponse<IEnumerable<UserToReturnDto>>> GetUsersbyplanid(Guid id) 
        {
            var user = await Mediator.Send(new GetUsersbyplanid(id));
            if (!user.IsSuccess)
            {
                return EndpointResponse<IEnumerable<UserToReturnDto>>.Fail(user.Message, user.StatusCode);
            }

            return EndpointResponse<IEnumerable<UserToReturnDto>>.Success(user.Data, user.Message, user.StatusCode);
        }

        [HttpPut]
        public async Task<EndpointResponse<bool>> UpdateUserProfileImage(Guid id,IFormFile formFile) 
        {
            var result = await Mediator.Send(new UpdateUserProfileOrchestrator(id, formFile));
            if (!result.IsSuccess)
            {
                return EndpointResponse<bool>.Fail(result.Message, result.StatusCode);
            }

            return EndpointResponse<bool>.Success(result.Data, result.Message, result.StatusCode);
        }
    }
}
