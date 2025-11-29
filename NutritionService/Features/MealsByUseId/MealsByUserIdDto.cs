namespace NutritionService.Features.MealsByUseId
{
    public class MealsByUserIdDto
    {
        public int UserId { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
