namespace UserProfileService.Shared.Entites
{
    public class UserProgress: BaseEntity
    {
        public Guid UserId { get; set; }
        public decimal CurrentWeight { get; set; }
        public DateTime MeasurementDate { get; set; }
    }
}