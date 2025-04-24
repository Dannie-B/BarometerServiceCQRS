namespace BarometerService.Infrastructure.Data;

using System;
using System.Data.Common;
using System.Diagnostics;

using Microsoft.EntityFrameworkCore.Diagnostics;

internal sealed class PerformanceInterceptor(ILogger<PerformanceInterceptor> logger)
  : DbCommandInterceptor
{
  private const long QuerySlowThreshold = 100; // milliseconds

  public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
  {
    Stopwatch stopwatch = Stopwatch.StartNew();

    InterceptionResult<DbDataReader> originalResult = base.ReaderExecuting(command, eventData, result);

    stopwatch.Stop();
    if (stopwatch.ElapsedMilliseconds > QuerySlowThreshold)
    {
      logger.LogInformation("Slow Query Detected: {command}", command.CommandText);
    }

    return originalResult;
  }
}