using System.Text.Json.Serialization;

namespace IdentityService.Features.Authantication
{
    public class AuthModel
    {
        public bool IsAuthenticated { get; set; }
        public string? Token { get; set; }
        public DateTime? TokenExpiresOn { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiration { get; set; }

    }
}
