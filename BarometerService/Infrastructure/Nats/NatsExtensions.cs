using NATS.Client.Hosting;
using Nats.Provider;
using BarometerService.Domain.Abstract;
namespace BarometerService.Infrastructure.Nats
{
    public static class NatsExtensions
    {
        public static IServiceCollection AddCommandProvider<T>(this IServiceCollection services, IConfiguration configuration)
          where T : class
        {
            NatsServiceConfig? config = configuration.GetSection("Nats").Get<NatsServiceConfig>()
              ?? throw new ArgumentException("NATS config not found");
            _ = services.AddSingleton(config);
            _ = services.AddNats();
            _ = services.AddSingleton<ICommandService, CommandService>();
            _ = services.AddSingleton<NatsChannel<T>>();
            _ = services.AddSingleton<NatsReader<T>>();
            _ = services.AddSingleton<NatsWriter<T>>();

            return services;
        }
    }
}
