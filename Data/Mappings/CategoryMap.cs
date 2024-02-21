using BlogEFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogEFCore.Data.Mappings;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Table
        builder.ToTable("Categories");

        // PrimaryKey
        builder.HasKey(c => c.Id);

        // Identity
        // builder.Property(c => c.Id).ValueGeneratedOnAdd().UseIdentityColumn();

        // Properties
        builder.Property(c => c.Name).IsRequired().HasColumnName("Name").HasDefaultValue("");
        //.HasColumnType("NVARCHAR").HasMaxLength(80);
        builder.Property(c => c.Slug).IsRequired().HasColumnName("Slug").HasDefaultValue("");

        // Index
        // builder.HasIndex(c => c.Slug, "IX_Category_Slug").IsUnique();
    }
}
