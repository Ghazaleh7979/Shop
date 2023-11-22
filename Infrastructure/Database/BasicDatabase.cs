using Domain.Models;
using Infrastructure.Database.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class BasicDatabase : DbContext
{
    public BasicDatabase(DbContextOptions<BasicDatabase> options) : base(options)
    {
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        var entries = ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            entry.State = EntityState.Detached;
        }

        return result;
    }
    
    public DbSet<Product>? Products { get; set; }
    public DbSet<User>? Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
    }

}