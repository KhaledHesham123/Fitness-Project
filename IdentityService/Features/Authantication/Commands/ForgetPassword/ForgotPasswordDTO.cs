namespace IdentityService.Features.Authantication.ForgotPassword
{
    public class ForgotPasswordDTO
    {
        public string Email {  get; set; }
        public int ExpirationInMinutes { get; set; }
    }
}
