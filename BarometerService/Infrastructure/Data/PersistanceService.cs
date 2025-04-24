namespace BarometerService.Infrastructure.Data;

using BarometerService.Domain.Abstract;
using BarometerService.Domain.Model;
using Common.K8s.Monitoring.Monitoring;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

internal sealed class PersistanceService(ILogger<PersistanceService> logger,
  BarometerContext context,
  RequestCounterMetrics metrics
  )
  : ICommandService
{
    public Task<IEnumerable<Measure>> GetPersistedMeasuresByDays(int days)
    {
        Stopwatch sw = Stopwatch.StartNew();

        IEnumerable<Measure> measures = context.Measures
          .Where(m => m.Registered > DateTimeOffset.Now.AddDays(-days))
          .AsNoTracking()
          .OrderBy(m => m.Id)
          .AsEnumerable();

        sw.Stop();
        metrics.RequestOccured("BarometerService.BarometerByDays", 1, (int)sw.ElapsedMilliseconds);
        logger.LogInformation("GetMeasuresByDays elapsed: {elapsed}", sw.Elapsed);

        return Task.FromResult(measures);
    }

    public async Task<Measure?> GetLatestPersistedMeasure()
    {
        Stopwatch sw = Stopwatch.StartNew();

        Measure? measure = await context.Measures
          .AsNoTracking()
          .OrderByDescending(m => m.Registered)
          .FirstOrDefaultAsync();

        sw.Stop();
        metrics.RequestOccured("BarometerService.barometerlatest", 1, (int)sw.ElapsedMilliseconds);
        logger.LogInformation("GetLatestMeasure elapsed: {elapsed}", sw.Elapsed);

        return measure;
    }

    public async Task<Measure> PostMeasure(Measure measure)
    {
        _ = context.Add(measure);
        _ = await context.SaveChangesAsync();

        return measure;
    }
}
