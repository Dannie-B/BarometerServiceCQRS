namespace BarometerService.SignalR;

using BarometerService.Domain.Contracts;

public interface IBarometerHubClient
{
    Task SendBarometerUpdate(MeasureResponse measure);
}