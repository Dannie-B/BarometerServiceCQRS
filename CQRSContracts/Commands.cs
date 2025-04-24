namespace CQRSContracts;
public sealed record NatsCommand(Guid Id, DateTimeOffset Timestamp, object Command, NatsCommandType commandType);

public sealed record AddMeasureCommand(int Id, double Temperature, double Pressure, double Humidity, double RefTemperature, double RefPressure, double RefHumidity, double Altitude, string? IpAddress, string? MacAddress, string? HostName)
{
    public DateTimeOffset Registered { get; set; } = DateTimeOffset.Now;
}
public enum NatsCommandType
{
  PostMeasure
}
