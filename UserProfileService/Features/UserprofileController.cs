using System.Collections.Generic;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserProfileService.Contract;
using UserProfileService.Features.Commend;
using UserProfileService.Features.Query;
using UserProfileService.Shared.Dto;
using UserProfileService.Shared.Response;

namespace UserProfileService.Features
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize] // Temporarily disabled for testing
    public class UserprofileController : ControllerBase
    {
        private readonly IAddUserprofileQrcs _dd;
        private readonly IMediator mediator1;
        private readonly IUpdateUserProfileQrccs _update;
        private readonly HttpClient _http;

        public UserprofileController(IMediator mediator ,IAddUserprofileQrcs add, IUpdateUserProfileQrccs update,HttpClient http)
        {
            mediator1 = mediator;
            _dd = add;
            _update = update;
            _http = http;
        }

        [HttpGet]
        public async Task<EndpointResponse<USerprofileDTo>> GetByUsername()
        {
            var model = await mediator1.Send(new GetUserprofile(getId()));

            if (!model.IsSuccess)
                return EndpointResponse<USerprofileDTo>.Fail(model.Message, model.StatusCode);

            return Shared.Response.EndpointResponse<USerprofileDTo>.Success(model.Data);

        }
        [HttpGet("UserProgress")]
        public  async Task<EndpointResponse<PagedResult<USerProgressDTo>>> GetByUSerProgress([FromQuery] USerProgressQueryParameters queryParameters)
        {
            var model = await mediator1.Send(new GetUserProgress(getId(), queryParameters.PageNumber, queryParameters.PageSize, queryParameters.SearchWeight, queryParameters.Date));

            if (!model.IsSuccess)
                return EndpointResponse<PagedResult<USerProgressDTo>>.Fail(model.Message, model.StatusCode);

            return Shared.Response.EndpointResponse<PagedResult<USerProgressDTo>>.Success(model.Data, "scusseful", 200);

        }
        [HttpPost]
        public async Task<EndpointResponse<USerprofileDTo>> AddUserprofile([FromForm] AddUserProfileRequest request)
        {
             var model = await _dd.AddUserProfileAsync(getId(), request);
            return EndpointResponse<USerprofileDTo>.Success(model.Data, "scusseful", 200);
        }

        [HttpPost("AddUsreProgres")]
        public async Task<EndpointResponse<USerProgressDTo>> AddUsreProgres([FromForm] USerProgressDTo request)
        {
            var newmodel = await mediator1.Send(new AddUsreProgress(getId(),request.CurrentWeight,request.MeasurementDate));

            return EndpointResponse<USerProgressDTo>.Success(newmodel.Data, "scusseful", 200);
        }
        [HttpPut]
        public async Task<EndpointResponse<USerprofileDTo>> UpdateUSerProfilePicturecs([FromForm] AddUserProfileRequest request)
        {
            var model = await _update.UpdateUserprofileEditAsync(getId(), request);
            if (model is null)
                return EndpointResponse<USerprofileDTo>.Fail(model.Message, model.StatusCode);

            // Get updated user profile
            return EndpointResponse<USerprofileDTo>.Success(model.Data, "scusseful", 200);
        }
        [HttpPut("resetPasswords")]
        public async Task<EndpointResponse<string>> UpdateUSerProfilePassword([FromForm] ResetPassward request)
        {

            var result = await _update.UpdateUserprofilePassword(getId(), request, _http);

            if (!result.IsSuccess)
                return EndpointResponse<string>.Fail(result.Message, result.StatusCode);

            return EndpointResponse<string>.Success("Password updated successfully", "scusseful", 200);
        }

        private Guid getId() 
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid parsedId))
            {
                return Guid.Parse("00000000-0000-0000-0000-000000000001");
            }
            return parsedId;
        }
    }
}
