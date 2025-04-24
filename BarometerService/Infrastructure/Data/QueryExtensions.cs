using BarometerService.Domain.Abstract;
using MySql.Data.MySqlClient;

namespace BarometerService.Infrastructure.Data
{
    public static class QueryExtensions
    {
        public static IServiceCollection AddQueryProvider(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.AddTransient(x => new MySqlConnection(configuration.GetConnectionString("DefaultConnection")));
            _ = services.AddSingleton<IQueryService, QueryService>();

            return services;
        }
    }
}
