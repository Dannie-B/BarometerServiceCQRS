namespace BarometerService.Domain;

using Microsoft.Extensions.DependencyInjection;

using BarometerService.Domain.Abstract;
using BarometerService.Domain.Services;
using BarometerService.Domain.Configuration;
using BarometerService.Infrastructure.Data;

internal static class DomainExtensions
{
    public static IHostApplicationBuilder AddAppDomain(this IHostApplicationBuilder builder)
    {
        _ = builder.Services.AddAppDomain(builder.Configuration);

        return builder;
    }

    public static IServiceCollection AddAppDomain(this IServiceCollection services,
      IConfiguration configuration)
    {
        _ = services.Configure<BarometerConfiguration>(options => configuration.GetSection("BarometerConfiguration").Bind(options))
        .AddScoped<IDomainCommandService, DomainCommandService>()
        .AddScoped<IDomainQueryService, DomainQueryService>()
        .AddScoped<IQueryService, QueryService>();

        return services;
    }
}
