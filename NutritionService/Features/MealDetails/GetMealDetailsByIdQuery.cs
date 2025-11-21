using MediatR;
using NutritionService.Shared.Response;
namespace NutritionService.Features.MealDetails
{
    public record GetMealDetailsByIdQuery(int MealId) : IRequest<ResponseResult<MealDetailsModelView>>;
}
