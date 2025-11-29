using UserProfileService.Entity;

namespace UserProfileService.Entites
{
    public class UserProgress: BaseEntity
    {
        public Guid UserId { get; set; }
        public decimal CurrentWeight { get; set; }
        public DateOnly MeasurementDate { get; set; }  //filter
    }
}