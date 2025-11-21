using MediatR;
using Microsoft.AspNetCore.Mvc;
using NutritionService.Features.MealDetails;
using NutritionService.Shared.Response;
using System.Threading.Tasks;

namespace NutritionService.Features.MealDetails
{
    [ApiController]
    [Route("api/[controller]")]
    public class MealController(IMediator _mediator) :ControllerBase
    {
        [HttpGet("details")]
        public async Task<ActionResult<ResponseResult<MealDetailsModelView>>> GetMealDetailsById([FromQuery] int mealId)
        {
            // Implementation for retrieving meal details by ID
            var result = await _mediator.Send(new GetMealDetailsByIdQuery(mealId));
            if (!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
    }
}
