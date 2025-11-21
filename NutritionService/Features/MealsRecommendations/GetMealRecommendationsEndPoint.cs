using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace NutritionService.Features.MealsRecommendations
{
    [ApiController]
    [Route("api/[controller]")]
    public class MealController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("recommendations")]
        public async Task<ActionResult<MealRecommendationModelView>> GetRecommendation([FromQuery]MealRecommendationDto mealRecommendationDto)
        {
            var result =await _mediator.Send(new GetMealRecommendationsQuery(mealRecommendationDto.MealType,mealRecommendationDto.pageNumber,mealRecommendationDto.pageSize,mealRecommendationDto.maxCalories,mealRecommendationDto.minProtein));
            if(result.Success)
                return Ok(result);
            return NotFound(result);
        }
    }
}
