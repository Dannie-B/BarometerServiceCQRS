namespace BarometerService.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

internal sealed class Design
  : IDesignTimeDbContextFactory<BarometerContext>
{
  public BarometerContext CreateDbContext(string[] args)
  {
    IConfiguration configuration = new ConfigurationBuilder()
      .AddUserSecrets<BarometerContext>()
      .Build();

    string? connectionString = configuration.GetConnectionString("DefaultConnection");

    ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);
    DbContextOptionsBuilder<BarometerContext> optionsBuilder = new();
    _ = optionsBuilder.UseMySql(connectionString, serverVersion)
        //Since this is executed only in designtime it's safe to enable sensitive data logging
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors();

    return new BarometerContext(optionsBuilder.Options);
  }
}
