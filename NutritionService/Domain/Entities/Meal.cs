using NutritionService.Domain.Enums;

namespace NutritionService.Domain.Entities
{
    public class Meal : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public MealType MealType { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Beginner;
        public int Calories { get; set; }
        public double Protein { get; set; }
        public double Carbohydrates { get; set; } 
        public double Fat { get; set; } 
        public string ImageUrl { get; set; } = string.Empty;
        public bool isPremium { get; set; } = false;
        public  ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    }
}
