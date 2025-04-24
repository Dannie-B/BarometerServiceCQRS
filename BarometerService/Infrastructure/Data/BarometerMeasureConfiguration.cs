namespace BarometerService.Infrastructure.Data;

using BarometerService.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class BarometerMeasureConfiguration
  : IEntityTypeConfiguration<Measure>
{
    public void Configure(EntityTypeBuilder<Measure> builder)
    {
        builder.HasKey(e => e.Id);
        builder.ToTable("Measures");

        builder.HasIndex(e => e.Registered);
    }
}