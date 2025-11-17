using IdentityService.Features.Authantication;
using IdentityService.Features.Authantication.Commands.AddRefreshToken;
using IdentityService.Features.Authantication.Queries.GetPermissionsByUserId;
using IdentityService.Features.Authantication.Queries.GetRefreshTokenByUserId;
using IdentityService.Features.Authantication.Queries.GetRolesByUserId;
using IdentityService.Features.Shared.CheckExist;
using IdentityService.Shared.Entities;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Shared.JWTService
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        private readonly IMediator _mediator;

        public JwtService(IConfiguration config, IMediator mediator)
        {
            _config = config;
            _mediator = mediator;
        }
        public async Task<AuthModel> GenerateAccessTokenAsync(User user)
        {
            var authModel = new AuthModel();

            // ========================
            //       CREATE CLAIMS
            // ========================
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Aud, _config["JWT:Audience"]),
                new Claim(JwtRegisteredClaimNames.Iss, _config["JWT:Author"])
            };

            // Add Roles
            var userRoles = (await _mediator.Send(new GetRolsByUserIdQuery(user.Id))).Data.Select(s => s.RoleName);
            foreach (var role in userRoles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            // Add Permissions
            var userPermissions = (await _mediator.Send(new GetPermissionsByUserIdQuery(user.Id))).Data.Select(s => s.PermissionName);
            foreach (var perm in userPermissions)
                claims.Add(new Claim("permission", perm));

            // ========================
            //      CREATE ACCESS TOKEN
            // ========================
            var keyInByte = Encoding.ASCII.GetBytes(_config["JWT:Key"]);
            SymmetricSecurityKey key = new SymmetricSecurityKey(keyInByte);
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(30));

            var tokenHandler = new JwtSecurityTokenHandler();
            var accessToken = tokenHandler.WriteToken(jwt);

            authModel.Token = accessToken;

            // ========================
            //     REFRESH TOKEN FIX
            // ========================
            var refreshToken = (await _mediator.Send(new GetRefreshTokenByUserIdQuery(user.Id))).Data;
            if (refreshToken is null)
            {
                refreshToken = GenerateRefreshToken();
                refreshToken.UserId = user.Id;
                await _mediator.Send(new AddRefreshTokenCommand(refreshToken));
            }

            authModel.RefreshToken = refreshToken.Token;
            authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;

            return authModel;
        }

        public RefreshToken GenerateRefreshToken()
        {
            var random = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(random);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(random),
                CreatedAt = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(7)
            };
        }
    }
}
