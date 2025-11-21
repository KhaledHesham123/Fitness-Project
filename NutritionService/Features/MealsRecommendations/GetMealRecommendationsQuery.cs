using MediatR;
using NutritionService.Shared.Response;

namespace NutritionService.Features.MealsRecommendations
{
    public record GetMealRecommendationsQuery(string MealType , int pageNumber , int pageSize , int? maxCalories , int? minProtein) : IRequest<ResponseResult<List<MealRecommendationModelView>>>;   
}
