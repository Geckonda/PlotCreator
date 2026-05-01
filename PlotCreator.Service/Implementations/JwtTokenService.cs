using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;
using PlotCreator.Service.Interfaces;

namespace PlotCreator.Service.Implementations
{
    public sealed class JwtTokenService : IJwtTokenService
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly SymmetricSecurityKey _key;
        public int ExpiresInDays { get; }
        public string CookieName { get; }

        public JwtTokenService(IConfiguration config)
        {
            var section = config.GetSection("Jwt");
            _issuer = section["Issuer"] ?? "PlotCreator";
            _audience = section["Audience"] ?? "PlotCreator.Frontend";
            var keyText = section["Key"]
                ?? throw new InvalidOperationException("Jwt:Key is missing in configuration.");
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyText));
            ExpiresInDays = int.TryParse(section["ExpiresInDays"], out var d) ? d : 7;
            CookieName = section["CookieName"] ?? "pc_auth";
        }

        public string IssueToken(User user)
        {
            var role = user.RoleId switch
            {
                (int)UserRole.Admin => "Admin",
                (int)UserRole.Moderator => "Moderator",
                _ => "User"
            };

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("userId", user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login ?? string.Empty),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(ExpiresInDays),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
