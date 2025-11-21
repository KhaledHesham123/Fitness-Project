namespace NutritionService.Domain.Entities
{
    public class Ingredient : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;
        public int MealId { get; set; }
        public Meal Meal { get; set; }
    }
}
