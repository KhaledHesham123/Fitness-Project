using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkoutCatalogService.Features.Workout.DTOs;
using WorkoutCatalogService.Shared.Response;

namespace WorkoutCatalogService.Features.Workout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IMediator mediator;

        public WorkoutController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("GetAllWorkouts")] // GET: api/Workout/GetAllWorkouts
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
    }
}
