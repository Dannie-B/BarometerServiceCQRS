namespace BarometerService.SignalR;

using BarometerService.Domain.Contracts;
using Microsoft.AspNetCore.SignalR;

public sealed class BarometerHub : Hub<IBarometerHubClient>
{
    public async Task SendBarometerUpdate(MeasureResponse measure)
      => await Clients.All.SendBarometerUpdate(measure);
}