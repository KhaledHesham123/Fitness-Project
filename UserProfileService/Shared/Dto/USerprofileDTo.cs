
using UserProfileService.Entites;

namespace UserProfileService.Shared.Dto
{
    public class USerprofileDTo
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public FitnessGoal FitnessGoal { get; set; }
        public List<UserProgress> ProgressHistory { get; set; } = new();
    }
}
