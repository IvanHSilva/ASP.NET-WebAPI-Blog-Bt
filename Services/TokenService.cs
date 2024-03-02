using BlogEFCore.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog.Services; 

public class TokenService {

    public string GenerateToken(User user) {

        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
        SecurityTokenDescriptor tokenDescriptor = new(){
            Subject = new ClaimsIdentity(new Claim[]{
                new (ClaimTypes.Name, "ivanhenriques"), // User.Identity.Name
                new (ClaimTypes.Role, "user"),          // User.IsInRole
                new (ClaimTypes.Role, "admin"),
            }),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
