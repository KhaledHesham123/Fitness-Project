using NutritionService.Models.Enumeration;

namespace NutritionService.Models
{
    public class Meal : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public MealType MealType { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public int Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbohydrates { get; set; } 
        public decimal Fat { get; set; } 
        public string ImageUrl { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; }
      

    }
}
