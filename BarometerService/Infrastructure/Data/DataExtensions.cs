namespace BarometerService.Infrastructure.Data;

using Microsoft.Extensions.DependencyInjection;

using BarometerService.Domain.Abstract;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

internal static class DataExtensions
{
  public static IServiceCollection AddAppData(this IServiceCollection services, IConfiguration configuration)
  {
    _ = services.AddSingleton<PerformanceInterceptor>();
    _ = services.AddScoped<ICommandService, PersistanceService>();
   

        services.AddDbContext<BarometerContext>((sp,options) =>
    {
      string? connectionString = configuration.GetConnectionString("DefaultConnection");
      ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);
      _ = options.UseMySql(connectionString, serverVersion)
          .AddInterceptors(sp.GetRequiredService<PerformanceInterceptor>());
    });

    return services;
  }
  
  public static IHost UseAppData(this IHost host)
  {
    using IServiceScope scope = host.Services.CreateScope();
    BarometerContext context = scope.ServiceProvider.GetRequiredService<BarometerContext>();
    context.UpdateDb();

    return host;
  }
}
