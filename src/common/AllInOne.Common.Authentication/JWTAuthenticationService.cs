using AllInOne.Common.Authentication.Configuration;
using AllInOne.Common.Authentication.Helper;
using AllInOne.Domains.Core.Identity.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AllInOne.Common.Authentication
{
    public class JWTAuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationSettings _authenticationSettings;

        public JWTAuthenticationService(IOptions<AuthenticationSettings> authenticationSettings)
        {
            _authenticationSettings = authenticationSettings.Value;
        }

        public string GenerateToken(User user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _authenticationSettings.Issuer,
                Audience = _authenticationSettings.Audience,
                NotBefore = DateTime.UtcNow,
                IssuedAt = DateTime.UtcNow,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.RoleName),
                }),
                Expires = DateTime.UtcNow.AddDays(_authenticationSettings.ExpirationDurationInDays),
                SigningCredentials = JWTHelper.GetSigningCredentials(_authenticationSettings.SecretKey)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenId = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(tokenId);
        }
    }
}
