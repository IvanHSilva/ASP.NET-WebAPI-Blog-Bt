using System.ComponentModel.DataAnnotations.Schema;

namespace BlogEFCore.Models;

[Table("Roles")]
public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public IList<User> Users { get; set; } = null!;
}
