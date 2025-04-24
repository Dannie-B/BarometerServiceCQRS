namespace BarometerService.Infrastructure.Data;

using BarometerService.Domain.Model;

using System.Reflection;

using Microsoft.EntityFrameworkCore;

internal sealed class BarometerContext(DbContextOptions<BarometerContext> options)
  : DbContext(options)
{
  public DbSet<Measure> Measures => Set<Measure>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
    => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

  public Task UpdateDb()
  {
    if (Database.GetPendingMigrations().Any())
    {
      Database.Migrate();
    }

    return Task.CompletedTask;
  }
}
