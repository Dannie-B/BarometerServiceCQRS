using BarometerService.Domain.Abstract;
using BarometerService.Domain.Configuration;
using BarometerService.Domain.Model;
using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace BarometerService.Infrastructure.Data
{
    internal sealed class QueryService(
        IOptions<BarometerConfiguration> configurationOptions, MySqlConnection connection)
        : IQueryService
    {
        private readonly BarometerConfiguration configuration = configurationOptions.Value
        ?? throw new ArgumentNullException("Barometer configuration is null");

        public Task<Measure?> GetLatestPersistedMeasure()
        {
            IEnumerable<Measure>? measure = connection.Query<Measure>("SELECT * FROM Measures ORDER BY Registered DESC LIMIT 1;");

            return measure is null || measure.Count() == 0
              ? Task.FromResult<Measure?>(null)
              : measure.Count() > 1
              ? throw new InvalidOperationException("More than one user found")
              : Task.FromResult<Measure?>(measure.First());
        }

        public Task<IEnumerable<Measure>> GetPersistedMeasuresByDays()
        {
            IEnumerable<Measure>? measures = connection.Query<Measure>("Select * from Measures WHERE DateDiff(CURDATE(), Registered) <= @days", new { days = configuration.NrOfDays });

            return measures is not null && measures.Count() > 0
              ? Task.FromResult<IEnumerable<Measure>>(measures.ToList())
              : Task.FromResult<IEnumerable<Measure>>([]);
        }
    }
}
