namespace BarometerService.Domain.Model
{
    internal sealed class Measure
    {
        public required int Id { get; set; }
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        public double RefTemperature { get; set; }
        public double RefPressure { get; set; }
        public double RefHumidity { get; set; }
        public double Altitude { get; set; }
        public DateTimeOffset Registered { get; set; } = DateTimeOffset.Now;
        public string? IpAddress { get; set; }
        public string? MacAddress { get; set; }
        public string? HostName { get; set; }
    }
}
