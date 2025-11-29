namespace NutritionService.Shared.MessageBrocker.Messages
{
    public class UserTargetMessage : BasicMessage
    {
        public int UserId { get; set; }
        public int DailyCalorieTarget { get; set; }
        public double ProteinGoal { get; set; }
        public double CarbGoal { get; set; }
        public double FatGoal { get; set; }
    }

    /*
     {
       "UserId": 15,
       "DailyCalorieTarget": 2100,
       "ProteinGoal": 110,
       "CarbGoal": 250,
       "FatGoal": 70,
       "Type": "UserTargetMessage"
     } 
     */
}
