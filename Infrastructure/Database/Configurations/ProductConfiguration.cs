using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product", "Product").HasKey(product => product.Id);
        builder.HasQueryFilter(product => !product.IsDeleted);
        builder.HasIndex(product => product.ManufactureEmail).IsUnique();
        builder.HasIndex(product => product.ProduceDate).IsUnique();

        
        builder.Property(product => product.ProduceDate)
            .HasConversion(time => time.ToUniversalTime(), time => time.ToUniversalTime());

        builder
            .HasOne(product => product.User)
            .WithMany(user => user.Products)
            .HasForeignKey(user => user.UserId)
            .IsRequired(false);
    }
}