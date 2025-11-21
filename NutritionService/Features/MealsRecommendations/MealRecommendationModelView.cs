using NutritionService.Domain.Enums;

namespace NutritionService.Features.MealsRecommendations
{
    public class MealRecommendationModelView
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Calories { get; set; }
        public int Protein { get; set; }
        public int Carbs { get; set; }
        public int Fat { get; set; }
        public string ImageUrl { get; set; } = string.Empty;  
        public string VideoUrl { get; set; } = string.Empty;
        public MealType MealType { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public bool isPremium { get; set; }
    }
}
