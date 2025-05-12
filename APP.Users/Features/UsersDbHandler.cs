using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using APP.Users.Domain;
using CORE.APP.Features;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace APP.Users.Features
{
    public abstract class UsersDbHandler : Handler
    {
        protected readonly UsersDb _db;

        protected UsersDbHandler(UsersDb db) : base(new CultureInfo("en-US"))
        {
            _db = db;
        }
        protected virtual string CreateAccessToken(List<Claim> claims, DateTime expiration)
        {
            // Create signing credentials using the app's symmetric security key and HMAC SHA256 algorithm
            var signingCredentials = new SigningCredentials(AppSettings.SigningKey, SecurityAlgorithms.HmacSha256Signature);

            // Create the JWT token with issuer, audience, claims, and expiration
            var jwtSecurityToken = new JwtSecurityToken(AppSettings.Issuer, AppSettings.Audience, claims, DateTime.Now, expiration, signingCredentials);

            // Write the token to a string
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            return jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
        }

        /// <summary>
        /// Generates a list of claims based on the provided user object.
        /// </summary>
        /// <param name="user">The user for whom to generate claims.</param>
        /// <returns>A list of claims including Name, Role, and Id.</returns>
        protected virtual List<Claim> GetClaims(User user)
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("Id", user.Id.ToString()) // Custom claim for the user's ID
            };
        }

        /// <summary>
        /// Generates a secure, random refresh token encoded in Base64 format.
        /// This token is typically stored and used to issue a new access token after the current one expires.
        /// </summary>
        /// <returns>A Base64-encoded string representing the generated refresh token.</returns>
        protected virtual string CreateRefreshToken()
        {
            var bytes = new byte[32]; // 256-bit token size for strong entropy

            // Generate a cryptographically secure random number
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(bytes);
            }

            // Convert the random byte array to a Base64 string
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Extracts the <see cref="ClaimsPrincipal"/> from a given JWT access token without validating its expiration.
        /// This is useful for token renewal scenarios where the token may have expired but still needs to be parsed.
        /// </summary>
        /// <param name="accessToken">The JWT access token, optionally prefixed with "Bearer".</param>
        /// <returns>
        /// A <see cref="ClaimsPrincipal"/> representing the user’s identity and claims.
        /// Returns null if the token is invalid or cannot be parsed.
        /// </returns>
        protected virtual ClaimsPrincipal GetPrincipal(string accessToken)
        {
            // Remove "Bearer " prefix if present
            accessToken = accessToken.StartsWith(JwtBearerDefaults.AuthenticationScheme) ?
                accessToken.Remove(0, JwtBearerDefaults.AuthenticationScheme.Length + 1) : accessToken;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, // Skips checking the token issuer
                ValidateAudience = false, // Skips checking the token audience
                ValidateLifetime = false, // Skips token expiration check
                ValidateIssuerSigningKey = true, // Ensures the token was signed with a valid key
                IssuerSigningKey = AppSettings.SigningKey // The key used to validate the signature
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            // Validate the token and extract claims
            var principal = jwtSecurityTokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);

            // Return null if token is invalid; otherwise return principal
            return securityToken is null ? null : principal;
        }
    }
}
