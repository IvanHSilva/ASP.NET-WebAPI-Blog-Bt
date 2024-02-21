using BlogEFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogEFCore.Data.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Table
        builder.ToTable("Users");

        // PrimaryKey
        builder.HasKey(u => u.Id);

        // Identity
        // builder.Property(u => u.Id).ValueGeneratedOnAdd().UseIdentityColumn();

        // Properties
        builder.Property(u => u.Name).IsRequired().HasColumnName("Name").HasDefaultValue("");
        //.HasColumnType("NVARCHAR").HasMaxLength(80);
        builder.Property(u => u.Email).IsRequired().HasColumnName("Email").HasDefaultValue("");
        builder.Property(u => u.PasswordHash).IsRequired().HasColumnName("PasswordHash").HasDefaultValue("");
        builder.Property(u => u.Bio).IsRequired().HasColumnName("Bio").HasDefaultValue("");
        builder.Property(u => u.Image).IsRequired().HasColumnName("Image").HasDefaultValue("");
        builder.Property(u => u.Slug).IsRequired().HasColumnName("Slug").HasDefaultValue("");

        // Index
        // builder.HasIndex(u => u.Slug, "IX_User_Slug").IsUnique();

        // Relations
        builder.HasMany(u => u.Roles).WithMany(u => u.Users)
        .UsingEntity<Dictionary<string, object>>("UsersRoles",
        role => role.HasOne<Role>().WithMany().HasForeignKey("RoleId")
        .HasConstraintName("FK_UserRole_RoleId").OnDelete(DeleteBehavior.Cascade),
        user => user.HasOne<User>().WithMany().HasForeignKey("UserId")
        .HasConstraintName("FK_UserRole_UserId").OnDelete(DeleteBehavior.Cascade)
        );
    }
}