using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using FleetFactory.Infrastructure.Identity;

namespace FleetFactory.Infrastructure.Services
{
    public class JwtService (IConfiguration _configuration,  UserManager<ApplicationUser> _userManager)
    {
        public async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //claims 
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id), //this is used to identify the user in the system and is a standard claim type for user id
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim("firstName", user.FirstName), 
                new Claim("lastName", user.LastName ?? "")
            }; //this is creating list of claim objects with user information to be attached to token; jwtregisterdclaimnames is standard constants

            //roles 
            var roles = await _userManager.GetRolesAsync(user);

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role)); //adding role claim for each role the user has
            }

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), //token valid for 1 hour
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token); //generating the token string from the token object


        }
    }
}