using MediatR;
using NutritionService.Shared.Response;

namespace NutritionService.Features.GetMealRecommendations
{
    public record GetMealRecommendationsQuery(string MealType , int pageNumber , int pageSize , int? maxCalories , int? minProtein) : IRequest<ResponseResult<List<MealRecommendationModelView>>>;   
}
