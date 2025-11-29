namespace NutritionService.Domain.Entities
{
    public class UserNutritionProfile : BaseEntity
    {
        public int UserId { get; set; }
        public int DailyCalorieTarget { get; set; }
        public double ProteinGoal { get; set; }
        public double CarbGoal { get; set; }
        public double FatGoal { get; set; }
    }
}
