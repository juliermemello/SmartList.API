using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartList.Domain.Entity;

namespace SmartList.Infrastructure.Mappings;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .IsRequired()
            .UseIdentityColumn();

        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Username)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(p => p.Password)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(p => p.Active)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(p => p.UpdatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.HasIndex(p => p.Name).HasDatabaseName("idx_users_name");
        builder.HasIndex(p => p.Username).HasDatabaseName("idx_users_username");
        builder.HasIndex(p => p.Email).HasDatabaseName("idx_users_email");
        builder.HasIndex(u => u.Email).IsUnique();
    }
}
