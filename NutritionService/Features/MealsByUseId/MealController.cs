using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace NutritionService.Features.MealsByUseId
{
    [ApiController]
    [Route("api/[controller]")]
    public class MealController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("RecommendationByUserId")]
        public async Task<IActionResult> GetMealsRecommendationByUserId([FromQuery] MealsByUserIdDto mealsByUserIdDto)
        {
            var result =await  _mediator.Send(new MealsByUserIdQuery(mealsByUserIdDto.UserId , mealsByUserIdDto.pageNumber , mealsByUserIdDto.pageSize));
            if(!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
