using BlogEFCore.Models;
using System.Security.Claims;

namespace Blog.Extensions; 

public static class RoleClaimsExtension {

    public static IEnumerable<Claim> GetClaims(this User user) {

        List<Claim> result = [
            new(ClaimTypes.Name, user.Email)
        ];
        result.AddRange(user.Roles.Select(role => new Claim(
            ClaimTypes.Role, role.Slug)));

        return result;
    }
}
