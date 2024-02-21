namespace BlogEFCore.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;

    public DateTime CreateDate { get; set; }
    public DateTime LastUpdateDate { get; set; }

    // public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    // public int AuthorId { get; set; }
    public User Author { get; set; } = null!;

    public IList<Tag> Tags { get; set; } = null!;
}

