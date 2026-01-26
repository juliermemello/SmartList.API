using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartList.Domain.Entity;

namespace SmartList.Infrastructure.Mappings;
internal class ListItemConfiguration : IEntityTypeConfiguration<ListItem>
{
    public void Configure(EntityTypeBuilder<ListItem> builder)
    {
        builder.ToTable("list_items");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .IsRequired()
            .UseIdentityColumn();

        builder.Property(li => li.Ean)
            .HasMaxLength(13);

        builder.Property(li => li.Quantity)
            .HasPrecision(18, 3);

        builder.Property(li => li.UnitPrice)
            .HasPrecision(18, 2);

        builder.Ignore(li => li.TotalPrice);

        builder.HasOne(li => li.ShoppingList)
            .WithMany(sl => sl.Items)
            .HasForeignKey(li => li.ShoppingListId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(li => li.Product)
            .WithMany()
            .HasForeignKey(li => li.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(p => p.UpdatedAt)
            .HasDefaultValueSql("GETDATE()");
    }
}
