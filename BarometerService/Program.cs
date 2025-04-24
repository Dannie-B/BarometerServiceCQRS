using Common.K8s.Api;
using Common.K8s.Application;
using Common.K8s.Logging;
using Common.K8s.Monitoring;
using Common.K8s.SignalR;
using BarometerService.Application.Endpoints;
using BarometerService.Domain;
using BarometerService.Infrastructure.Data;

using Scalar.AspNetCore;
using BarometerService.SignalR;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddCommonK8sApi();
builder.AddCommonK8sLogging();
builder.AddCommonK8sApplication(typeof(Program));
builder.AddCommonK8sSignalR();
builder.AddCommonK8sHealthChecks(checks =>
{
  //TODO Add custom healtchecks here
});
builder.AddCommonK8sMonitoring("BarometerService", meter =>
{
  //TODO Add custom metrics (meters, views etc) here
  _ = meter.AddMeter("Common.K8s.Monitoring.Monitoring.RequestCounterMetrics");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

//TODO Add custom services here:
builder.Services.AddQueryProvider(builder.Configuration);
builder.Services.AddAppData(builder.Configuration);
builder.AddAppDomain();

builder.Services.AddOutputCache();

WebApplication app = builder.Build();

app.UseCommonK8sApi()
  .UseCommonK8sSignalR<BarometerHub>()
  .UseCommonK8sHealthChecks()
  .UseCommonK8sMetrics()
  .UseCommonK8sPing();

if (app.Environment.IsDevelopment())
{
}
  _ = app.MapOpenApi();
  _ = app.MapScalarApiReference();

//TODO Add custom middleware here:

app.UseAppData();
app.UseAppEndpoints();

app.Run();
