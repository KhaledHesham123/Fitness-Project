namespace NutritionService.Models
{
    public class Ingredient : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;


        public int MealId { get; set; }
        public virtual Meal Meal { get; set; }
    }
}
