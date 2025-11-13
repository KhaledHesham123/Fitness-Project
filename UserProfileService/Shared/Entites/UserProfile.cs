namespace UserProfileService.Shared.Entites
{
    public enum Gender
    {
        Male,
        Female
    }

    public enum FitnessGoal
    {
        WeightLoss,
        MuscleGain,
        Endurance,
        Maintenance
    }

    public enum ActivityLevel
    {
        Sedentary,
        LightlyActive,
        ModeratelyActive,
        VeryActive
    }

    public class UserProfile:BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public FitnessGoal FitnessGoal { get; set; }
        public ActivityLevel ActivityLevel { get; set; }

        public List<UserProgress> ProgressHistory { get; set; } = new();
    }
}
