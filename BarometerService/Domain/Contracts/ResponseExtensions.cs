using BarometerService.Domain.Model;

namespace BarometerService.Domain.Contracts
{
    internal static class ResponseExtensions
    {
        public static MeasureResponse ToMeasureResponse(this Measure data)
        {
            return new MeasureResponse(
                data.Id,
                data.Temperature,
                data.Pressure,
                data.Humidity,
                data.Altitude,
                data.Registered
            );
        }
    }
}
