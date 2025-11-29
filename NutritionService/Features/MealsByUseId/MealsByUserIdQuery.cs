using MediatR;
using NutritionService.Domain.Entities;
using NutritionService.Features.MealsRecommendations;
using NutritionService.Shared.Interfaces;
using NutritionService.Shared.Response;
using Microsoft.EntityFrameworkCore;


namespace NutritionService.Features.MealsByUseId
{
    public record MealsByUserIdQuery(int userId , int pageNumber , int pageSize) : IRequest<ResponseResult<List<MealRecommendationModelView>>>;

    public class MealsByUserIdQueryHandler( IUserNutritionProfileRepository _userNutritionProfileRepository , IMealRepository _mealRepository) : IRequestHandler<MealsByUserIdQuery, ResponseResult<List<MealRecommendationModelView>>>
    {
        public async Task<ResponseResult<List<MealRecommendationModelView>>> Handle(MealsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var UserNutritionProfile =await _userNutritionProfileRepository.GetByIdAsync(request.userId);
            if (UserNutritionProfile == null)
                return ResponseResult<List<MealRecommendationModelView>>.FailResponse("User nutrition profile not found.");

            var meals = _mealRepository.GetAll();
            if (UserNutritionProfile.DailyCalorieTarget > 0)
                meals = meals.Where(m => m.Calories <= UserNutritionProfile.DailyCalorieTarget);
            if (UserNutritionProfile.ProteinGoal>0)
                meals = meals.Where(m => m.Protein >= UserNutritionProfile.ProteinGoal);
            if (UserNutritionProfile.CarbGoal > 0)
                meals = meals.Where(m => m.Carbohydrates >= UserNutritionProfile.CarbGoal);
            if (UserNutritionProfile.FatGoal > 0)
                meals = meals.Where(m => m.Fat >= UserNutritionProfile.FatGoal);

            var mealsResult = await meals.Skip((request.pageNumber - 1) * request.pageSize)
                                  .Take(request.pageSize)
                                  .Select(m => new MealRecommendationModelView
                                  {
                                      Id = m.Id,
                                      Name = m.Name,
                                      ImageUrl = m.ImageUrl,
                                      MealType = m.MealType
                                  })
                                  .ToListAsync();
            if (!mealsResult.Any())
                return ResponseResult<List<MealRecommendationModelView>>.FailResponse("No Recommended Meals Found");
            return ResponseResult<List<MealRecommendationModelView>>.SuccessResponse(mealsResult, "Meal recommendations fetched successfully");
        }
    }
}
