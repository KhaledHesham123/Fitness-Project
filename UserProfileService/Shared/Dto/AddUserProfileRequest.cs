
using UserProfileService.Entites;

namespace UserProfileService.Shared.Dto
{
    public class AddUserProfileRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public IFormFile ProfilePictureUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public FitnessGoal FitnessGoal { get; set; }
    }
}
