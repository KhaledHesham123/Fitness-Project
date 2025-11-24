using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkoutCatalogService.Features.Categories.CQRS.Commends;
using WorkoutCatalogService.Features.Plans.CQRS.Commends;
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

        [HttpGet("GetAllPlans")] //api/PLan/GetAllPlans
        public async Task<ActionResult<EndpointResponse<IEnumerable<PalnToReturnDto>>>> GetAllPlans()
        {
            var plansResult = await mediator.Send(new GetAllplansQuery());

            var response = new EndpointResponse<IEnumerable<PalnToReturnDto>>
            {
                IsSuccess = plansResult.IsSuccess,
                Message = plansResult.Message,
                Data = plansResult.Data
            };

            if (!plansResult.IsSuccess)
                return BadRequest(response);

            return Ok(response); 
        }


        [HttpGet("Getplanbyid")] //api/PLan/Getplanbyid
        public async Task<ActionResult<EndpointResponse<PalnToReturnDto>>> Getplanbyid(Guid id)
        {
            var plansResult = await mediator.Send(new GetPlanbyidQyery(id));

            var response = new EndpointResponse<PalnToReturnDto>
            {
                IsSuccess = plansResult.IsSuccess,
                Message = plansResult.Message,
                Data = plansResult.Data
            };

            if (!plansResult.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpPost("Addplan")] //api/PLan/Addplan
        public async Task<ActionResult<EndpointResponse<Guid>>> addplan(AddplanDto dto) 
        {
            var addplanresult= await mediator.Send(new AddPlanCommend(dto.id,dto.Name,dto.Description,dto.DifficultyLevel,dto.AssignedUserIds));
            var response = new EndpointResponse<Guid> {
                IsSuccess = addplanresult.IsSuccess,
                Message = addplanresult.Message,
                Data = addplanresult.Data
            };
            if (!response.IsSuccess)
            {
                return BadRequest(response);

            }
            return Ok(response);


        }
    }
}

