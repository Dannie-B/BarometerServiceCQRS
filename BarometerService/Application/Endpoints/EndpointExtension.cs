using BarometerService.Domain.Abstract;
using BarometerService.Domain.Contracts;
using BarometerService.Domain.Model;
using Common.K8s.Monitoring.Monitoring;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BarometerService.Application.Endpoints
{
    internal static class EndpointExtension
    {
        internal static IEndpointRouteBuilder UseAppEndpoints(this IEndpointRouteBuilder builder)
        {
            RouteGroupBuilder group = builder.MapGroup("/").WithTags("Barometer");
            group.MapPost("/barometer", async Task<Results<Created<MeasureResponse>, BadRequest>> ([FromBody] Measure measure,
              [FromServices] ILogger<Program> logger,
              [FromServices] IDomainCommandService service,
              [FromServices] RequestCounterMetrics metrics) =>
            {
                logger.LogInformation("Recieved post request from {host} - {ip}", measure.HostName, measure.IpAddress);

                Stopwatch sw = Stopwatch.StartNew();
                MeasureResponse data = await service.PostMeasure(measure);
                sw.Stop();

                logger.LogInformation("Post request completed in {time} ms", sw.ElapsedMilliseconds);
                metrics.RequestOccured("BarometerService.PostMeasure", 1, (int)sw.ElapsedMilliseconds);

                return data is null
                    ? TypedResults.BadRequest()
                    : TypedResults.Created(String.Empty, data);
            })
            .WithName("PostMeasure")
            .WithOpenApi();

            group.MapGet("/barometerByDays", async Task<Results<Ok<IEnumerable<MeasureResponse>>, NotFound>>([FromServices] ILogger<Program> logger,
              [FromServices] IDomainQueryService service,
              [FromServices] RequestCounterMetrics metrics) =>
            {
                Stopwatch sw = Stopwatch.StartNew();
                IEnumerable<MeasureResponse> data = await service.GetMeasuresByDays();
                sw.Stop();

                logger.LogInformation("GetMeasuresByDays request completed in {time} ms", sw.ElapsedMilliseconds);
                metrics.RequestOccured("BarometerService.GetMeasuresByDays", 1, (int)sw.ElapsedMilliseconds);

                return data is null 
                    ? TypedResults.NotFound() 
                    : TypedResults.Ok(data);
            })
            .WithName("barometerByDays")
            .WithOpenApi()
            .CacheOutput();

            group.MapGet("/barometerlatest", async Task<Results<Ok<MeasureResponse>, NotFound>>([FromServices] ILogger<Program> logger,
                [FromServices] IDomainQueryService service,
                [FromServices] RequestCounterMetrics metrics) =>
            {
                Stopwatch sw = Stopwatch.StartNew();
                MeasureResponse? data = await service.GetLatestMeasure();
                sw.Stop();

                logger.LogInformation("barometerlatest request completed in {time} ms", sw.ElapsedMilliseconds);
                metrics.RequestOccured("BarometerService.barometerlatest", 1, (int)sw.ElapsedMilliseconds);

                return data is null
                    ? TypedResults.NotFound()
                    : TypedResults.Ok(data);
            })
            .WithName("barometerlatest")
            .WithOpenApi()
            .CacheOutput();

            return builder;
        }
    }
}
