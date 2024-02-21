using BlogEFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogEFCore.Data.Mappings;

public class PostMap : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        // Table
        builder.ToTable("Posts");

        // PrimaryKey
        builder.HasKey(p => p.Id);

        // Identity
        // builder.Property(p => p.Id).ValueGeneratedOnAdd().UseIdentityColumn();

        // Properties
        builder.Property(p => p.Title).IsRequired().HasColumnName("Title").HasDefaultValue("");
        builder.Property(p => p.Slug).IsRequired().HasColumnName("Slug").HasDefaultValue("");
        builder.Property(p => p.Summary).IsRequired().HasColumnName("Summary").HasDefaultValue("");
        builder.Property(p => p.Body).IsRequired().HasColumnName("Body").HasDefaultValue("");
        builder.Property(p => p.CreateDate).IsRequired().HasColumnName("CreateDate").HasDefaultValue(DateTime.Now.ToUniversalTime());
        //.HasColumnType("SMALLDATETIME").HasDefaultValueSql("GETDATE()");
        builder.Property(p => p.LastUpdateDate).IsRequired().HasColumnName("LastUpdateDate").HasDefaultValue(DateTime.Now.ToUniversalTime());

        // Index
        // builder.HasIndex(p => p.Slug, "IX_Post_Slug").IsUnique();

        // Relations
        builder.HasMany(p => p.Tags).WithMany(p => p.Posts)
        .UsingEntity<Dictionary<string, object>>("PostsTags",
        post => post.HasOne<Tag>().WithMany().HasForeignKey("PostId")
        .HasConstraintName("FK_PostTag_PostId").OnDelete(DeleteBehavior.Cascade),
        tag => tag.HasOne<Post>().WithMany().HasForeignKey("TagId")
        .HasConstraintName("FK_PostTag_TagId").OnDelete(DeleteBehavior.Cascade)
        );
    }
}