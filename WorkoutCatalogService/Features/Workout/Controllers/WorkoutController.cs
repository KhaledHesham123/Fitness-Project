using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkoutCatalogService.Features.Workout.CQRS.Commend;
using WorkoutCatalogService.Features.Workout.CQRS.Orchestrators;
using WorkoutCatalogService.Features.Workout.DTOs;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Workout.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IMediator mediator;

        public WorkoutController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("GetAllWorkouts")] // GET: Workout/GetAllWorkouts
            public async Task<ActionResult<EndpointResponse<IEnumerable<WorkoutToreturnDto>>>> GetAllWorkouts()
            {
            var workoutsResult = await mediator.Send(new CQRS.Queries.GetAllWorkoutsQuery());

            var response = new EndpointResponse<IEnumerable<WorkoutToreturnDto>>
            {
                IsSuccess = workoutsResult.IsSuccess,
                Message = workoutsResult.Message,
                Data = workoutsResult.Data
            };

            if (!workoutsResult.IsSuccess)
                return BadRequest(response); 

            return Ok(response);
        }

        [HttpPost("AddWorkouts")] // POST: Workout/AddWorkout
        public async Task<ActionResult<EndpointResponse<IEnumerable<WorkoutToreturnDto>>>> AddWorkouts(IEnumerable<WorkoutToaddDto> workoutToAddDtos)
        {
            
            var result = await mediator.Send(new AddWorkoutsCommend(workoutToAddDtos));
            var response = new EndpointResponse<IEnumerable<WorkoutToreturnDto>>
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Data = result.Data
            };
            if (!result.IsSuccess)
                return BadRequest(response); 
            return CreatedAtAction(nameof(GetAllWorkouts), new { id = result.Data }, response);
        }


        [HttpGet("GetRecommendedWorkouts")] // GET:   /GetRecommendedWorkouts
        public async Task<ActionResult<EndpointResponse<IEnumerable<RecomendedWorkoutsDto>>>> GetRecommendedWorkoutsId( GetRecomendedWorkoutsDto Dto)
        {
            var workoutsResult = await mediator.Send(new GetRecommendedWorkoutsByPlan(Dto.planid,Dto.subcategoryname));
            var response = new EndpointResponse<IEnumerable<RecomendedWorkoutsDto>>
            {
                IsSuccess = workoutsResult.IsSuccess,
                Message = workoutsResult.Message,
                Data = workoutsResult.Data
            };
            if (!workoutsResult.IsSuccess)
                return BadRequest(response);
            return Ok(response);
        }
    }
}
