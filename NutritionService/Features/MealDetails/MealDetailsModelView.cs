using NutritionService.Domain.Entities;
using NutritionService.Domain.Enums;
using NutritionService.Features.MealDetails;

namespace NutritionService.Features.MealDetails
{
    public class MealDetailsModelView
    {
            public int Id { get; set; }
            public string MealType { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string ImageUrl { get; set; } = string.Empty;
            public string VideoUrl { get; set; } = string.Empty;
            public int Calories { get; set; }
            public decimal Protein { get; set; }
            public decimal Carbohydrates { get; set; }
            public decimal Fat { get; set; }
            public string DifficultyLevel { get; set; } = string.Empty;
            public bool isPremium { get; set; } = false;
            public List<IngredientModelView> Ingredients { get; set; } = new List<IngredientModelView>();

    }
}