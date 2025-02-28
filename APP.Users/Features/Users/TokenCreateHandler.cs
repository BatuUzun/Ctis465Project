using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace APP.Users.Features.Users
{
    public class TokenCreateRequest : IRequest<TokenCreateResponse>
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string Password { get; set; }
    }

    public class TokenCreateResponse : CommandResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

        public TokenCreateResponse(bool isSuccessful, string message = "", int id = 0) : base(isSuccessful, message, id)
        {
        }
    }

    public class TokenCreateHandler : UsersDbHandler, IRequestHandler<TokenCreateRequest, TokenCreateResponse>
    {
        public TokenCreateHandler(UsersDb db) : base(db)
        {
        }

        public async Task<TokenCreateResponse> Handle(TokenCreateRequest request, CancellationToken cancellationToken)
        {
            var user = await _db.Users.Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.UserName == request.UserName && u.Password == request.Password && u.IsActive);
            if (user is null)
                return new TokenCreateResponse(false, "Active user with the user name and password not found!");
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("Id", user.Id.ToString())
            };
            var signingKey = AppSettings.SigningKey;
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);
            var expiration = DateTime.Now.AddMinutes(AppSettings.ExpirationInMinutes);
            var jwtSecurityToken = new JwtSecurityToken(AppSettings.Issuer, AppSettings.Audience, claims, DateTime.Now, expiration, signingCredentials);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
            return new TokenCreateResponse(true, "Token created successfully.", user.Id)
            {
                Token = $"Bearer {token}",
                Expiration = expiration
            };
        }
    }
}
