namespace BarometerService.Domain.Services;

using BarometerService.Domain.Abstract;
using BarometerService.Domain.Configuration;
using BarometerService.Domain.Contracts;
using BarometerService.Domain.Model;
using BarometerService.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

internal sealed class DomainCommandService(ILogger<DomainCommandService> logger,
    ICommandService service,
    IHubContext<BarometerHub, IBarometerHubClient> hubContext)
  : IDomainCommandService
{
    public async Task<MeasureResponse> PostMeasure(Measure measurePost)
    {
        Measure measure = await service.PostMeasure(measurePost);

        await hubContext.Clients.All.SendBarometerUpdate(measure.ToMeasureResponse());

        return measure.ToMeasureResponse();
    }
}