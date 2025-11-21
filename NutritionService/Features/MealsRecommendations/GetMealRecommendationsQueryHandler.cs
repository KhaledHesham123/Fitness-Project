using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionService.Domain.Entities;
using NutritionService.Shared.Interfaces;
using NutritionService.Shared.Response;

namespace NutritionService.Features.MealsRecommendations
{
    public class GetMealRecommendationsQueryHandler(IMealRepository _repository) : IRequestHandler<GetMealRecommendationsQuery, ResponseResult<List<MealRecommendationModelView>>>
    {
        public async Task<ResponseResult<List<MealRecommendationModelView>>> Handle(GetMealRecommendationsQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.GetAll();
            if (!string.IsNullOrWhiteSpace(request.MealType))
                query = query.Where(m => m.MealType.ToString() == request.MealType);
            
            if (request.maxCalories.HasValue)
                 query = query.Where(m => m.Calories <= request.maxCalories);

            if (request.minProtein.HasValue)
                    query = query.Where(m => m.Protein >= request.minProtein);

            var meals =await query.Skip((request.pageNumber - 1) * request.pageSize)
                                  .Take(request.pageSize)
                                  .Select(m => new MealRecommendationModelView
                                  {
                                        Id = m.Id,
                                        Name = m.Name,
                                        ImageUrl = m.ImageUrl,
                                        MealType = m.MealType
                                  })
                                  .ToListAsync();
            if (!meals.Any())
                return ResponseResult<List<MealRecommendationModelView>>.FailResponse("No Recommended Meals Found");
            return ResponseResult<List<MealRecommendationModelView>>.SuccessResponse(meals, "Meal recommendations fetched successfully");

        }
    }
}
