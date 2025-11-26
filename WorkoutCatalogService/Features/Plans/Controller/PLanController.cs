using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkoutCatalogService.Features.Categories.CQRS.Commends;
using WorkoutCatalogService.Features.Plans.CQRS.Commends;
using WorkoutCatalogService.Features.Plans.CQRS.Orchestrator;
using WorkoutCatalogService.Features.Plans.CQRS.Quries;
using WorkoutCatalogService.Features.Plans.DTOs;
using WorkoutCatalogService.Features.PlanWorkouts.DTOS;
using WorkoutCatalogService.Features.Workout.DTOs;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Plans.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class PLanController : ControllerBase
    {
        private readonly IMediator mediator;

        public PLanController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("GetAllPlans")] //PLan/GetAllPlans
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


        [HttpGet("Getplanbyid")] //PLan/Getplanbyid
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


        [HttpPost("Addplan")] //PLan/Addplan
        public async Task<ActionResult<EndpointResponse<Guid>>> addplan(AddplanDto dto) 
        {
            var addplanresult= await mediator.Send(new AddPlanCommend(dto.Name,dto.Description,dto.DifficultyLevel,dto.AssignedUserIds));
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


        [HttpPost("CreateFullWorkoutPlan")] //PLan/CreateFullWorkoutPlan
        public async Task<ActionResult<EndpointResponse<Guid>>> CreateFullWorkoutPlan(CreateFullWorkoutPlanRequest request )
        {
            var CreatFullWorkoutPlanResult = await mediator.Send(new CreateFullPlanOrchestrator(request.Plan, request.PlanWorkouts, request.Workouts));
            var response = new EndpointResponse<Guid>
            {
                IsSuccess = CreatFullWorkoutPlanResult.IsSuccess,
                Message = CreatFullWorkoutPlanResult.Message,
                Data = CreatFullWorkoutPlanResult.Data
            };
            if (!response.IsSuccess)
            {
                return BadRequest(response);

            }
            return Ok(response);


        }


        [HttpGet("GetPLansWithUsersIds")] ///PLan/GetPLansWithUsersIds
        public async Task<ActionResult<EndpointResponse<IEnumerable<PalnToReturnDto>>>> GetPLansWithUsersIds([FromQuery]Guid id)
        {
            var GetPlansResult = await mediator.Send(new GetPlansWithUserIdOrchestrator(id));
            var response = new EndpointResponse<IEnumerable<PalnToReturnDto>>
            {
                IsSuccess = GetPlansResult.IsSuccess,
                Message = GetPlansResult.Message,
                Data = GetPlansResult.Data
            };
            if (!response.IsSuccess)
            {
                return BadRequest(response);

            }
            return Ok(response);


        }
    }
}

