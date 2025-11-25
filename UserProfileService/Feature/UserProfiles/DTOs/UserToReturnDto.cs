using UserProfileService.Shared.Entites;

namespace UserProfileService.Feature.UserProfiles.DTOs
{
    public class UserToReturnDto
    {
        public Guid Id { get; set; }    
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public string FitnessGoal { get; set; }

        public Guid? planid { get; set; } = null;

    }
}
