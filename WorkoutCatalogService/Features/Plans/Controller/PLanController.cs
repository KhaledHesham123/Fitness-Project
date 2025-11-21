using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkoutCatalogService.Features.Plans.CQRS.Quries;
using WorkoutCatalogService.Features.Plans.DTOs;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Plans.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PLanController : ControllerBase
    {
        private readonly IMediator mediator;

        public PLanController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("GetAllPlans")]
        public async Task<ActionResult<EndpointResponse<IEnumerable<PalnToReturnDto>>>> GetAllPlans()
        {
            var plansResult = await mediator.Send(new GetAllplansCommend());

            // تحويل من RequestResponse -> EndpointResponse
            var response = new EndpointResponse<IEnumerable<PalnToReturnDto>>
            {
                IsSuccess = plansResult.IsSuccess,
                Message = plansResult.Message,
                Data = plansResult.Data
            };

            if (!plansResult.IsSuccess)
                return BadRequest(response); // 400، أو ممكن NotFound(response) حسب الحالة

            return Ok(response); // 200
        }
    }
}

