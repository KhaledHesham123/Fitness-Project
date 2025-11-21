using MediatR;
using NutritionService.Features.MealDetails;
using NutritionService.Shared.Interfaces;
using NutritionService.Shared.Response;

namespace NutritionService.Features.MealDetails
{
    public class GetMealDetailsByIdQueryHandler(IMealRepository _mealRepository) : IRequestHandler<GetMealDetailsByIdQuery, ResponseResult<MealDetailsModelView>>
    {
        public async Task<ResponseResult<MealDetailsModelView>> Handle(GetMealDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            var meal =await _mealRepository.GetByConditionWithIncludesAsync(m=> m.Id == request.MealId, m => m.Ingredients);
            if (meal == null)
                 return ResponseResult<MealDetailsModelView>.FailResponse("Meal not found");
            var mealDetails = new MealDetailsModelView
            {
                Id = meal.Id,
                MealType = meal.MealType.ToString(),
                Name = meal.Name,
                Description = meal.Description,
                ImageUrl = meal.ImageUrl,
                VideoUrl = meal.VideoUrl,
                Calories = meal.Calories,
                Protein = meal.Protein,
                Carbohydrates = meal.Carbohydrates,
                Fat = meal.Fat,
                DifficultyLevel = meal.DifficultyLevel.ToString(),
                isPremium = meal.isPremium,
                Ingredients = meal.Ingredients?.Select(i => new IngredientModelView
                {
                    Id = i.Id,
                    Name = i.Name,
                    Quantity = i.Quantity
                }).ToList() ?? new List<IngredientModelView>()
            };
            return ResponseResult<MealDetailsModelView>.SuccessResponse(mealDetails , "Get Meal Details Successfully");
        }
    }
}
