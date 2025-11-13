namespace IdentityService.Domain.Entities
{
    public class UserToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredDate { get; set; }

    }
}
