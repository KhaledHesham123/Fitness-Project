namespace NutritionService.Features.MealsRecommendations
{
    public class MealRecommendationDto
    {
        public string MealType { get; set; } = string.Empty;
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 20;
        public int? maxCalories { get; set; }
        public int? minProtein { get; set; }
    }
}
