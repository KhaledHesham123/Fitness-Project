namespace NutritionService.Shared.MessageBrocker.Messages.UserCalorieTarget
{
    public class UserCalorieTargetMessage : BasicMessage
    {
        public int UserId { get; set; }
        public double DailyCalorieTarget { get; set; }
        public double ProteinGoal { get; set; }
        public double CarbGoal { get; set; }
        public double FatGoal { get; set; }
    }
}
