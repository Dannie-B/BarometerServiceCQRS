namespace BarometerService.Domain.Contracts;

public sealed record MeasureResponse(int Id, double Temperature, double Pressure,
    double Humidity, double Altitude, DateTimeOffset Registered);
