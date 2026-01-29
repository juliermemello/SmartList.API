using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartList.Domain.Entity;

namespace SmartList.Infrastructure.Mappings;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .IsRequired()
            .UseIdentityColumn();

        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Icon)
            .HasMaxLength(100);

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(p => p.UpdatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(p => p.User)
            .WithMany(u => u.Categories)
            .HasForeignKey(sl => sl.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(p => p.Name).HasDatabaseName("idx_categories_name");
    }
}
