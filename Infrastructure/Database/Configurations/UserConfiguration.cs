using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User", "User").HasKey(user => user.Id);
        builder.HasQueryFilter(user => !user.IsDeleted);
        builder.HasIndex(user => user.Username).IsUnique();

        builder.Property(user => user.Username).IsRequired().IsUnicode(false);
        builder.Property(user => user.Email).IsRequired().IsUnicode(false);
        builder.Property(user => user.PasswordHash).IsRequired().IsUnicode(false);
        
        builder.Property(user => user.InsertTime)
            .HasConversion(time => time.ToUniversalTime(), time => time.ToUniversalTime());
    }
}