using IdentityService.Features.Authantication;
using IdentityService.Features.Authantication.Commands.AddRefreshToken;
using IdentityService.Features.Authantication.Queries.GetPermissionsByUserId;
using IdentityService.Features.Authantication.Queries.GetRefreshTokenByUserId;
using IdentityService.Features.Authantication.Queries.GetRolesByUserId;
using IdentityService.Features.Authantication.Queries.GetUserByRefreshToken;
using IdentityService.Shared.Entities;
using IdentityService.Shared.Interfaces;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Shared.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IMediator _mediator;

        public AuthService(IConfiguration config, IMediator mediator)
        {
            _config = config;
            _mediator = mediator;
        }

        public async Task<string> GeneratePasswordHashAsync(string realPassword)
        {
            return await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(realPassword));
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await Task.Run(() => BCrypt.Net.BCrypt.Verify(password, user.HashPassword));
        }

        private async Task ChangePasswordAsync(int userId, string password)
        {
            throw new NotImplementedException();
        }
        public async Task<AuthModel> GenerateTokensAsync(User user)
        {
            var authModel = new AuthModel
            {
                IsAuthenticated = true
            };

            // ACCESS TOKEN =======================
            var jwt = await CreateJwtTokenAsync(user);
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwt); ;
            authModel.TokenExpiresOn = jwt.ValidTo;

            // REFRESH TOKEN ======================
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
        public async Task<AuthModel> RefreshTokenAsync(string refreshToken)
        {
            var user = await _mediator.Send(new GetUserByRefreshTokenQuery(refreshToken));
            if (!user.Success)
                return new AuthModel { Message = user.Message };

            var token = (await _mediator.Send(new GetRefreshTokenByUserIdQuery(user.Data.Id))).Data;
            if (!token.IsActive)
                return new AuthModel { Message = "Invalid refresh token" };

            token.RevokedOn = DateTime.UtcNow;
            var newTokens = await GenerateTokensAsync(user.Data);

            return newTokens;
        }
        private RefreshToken GenerateRefreshToken()
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

        private async Task<JwtSecurityToken> CreateJwtTokenAsync(User user)
        {
            // CREATE CLAIMS ========================
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

            // CREATE ACCESS TOKEN ========================
            var keyInByte = Encoding.ASCII.GetBytes(_config["JWT:Key"]);
            SymmetricSecurityKey key = new SymmetricSecurityKey(keyInByte);
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["jwt:ExpirationInMinutes"])));

            return jwt;
        }
    }
}
