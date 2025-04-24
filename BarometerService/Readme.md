### Regarding this template
This template is using all Common.K8s.* nugets that are hosted in our Azure Devops.  To be able to use the template you must have the rights to pull from that nuget feed, the nuget source for it is:  
https://lennartajansson.pkgs.visualstudio.com/_packaging/Kubernetes/nuget/v3/index.json.  

Some other things to modify in the application:  

First of all, add a user-secrets file to the project. (Sample in appsettings.json)  
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your connectionstring to MySql"
  }
}
```  

Then verify your CORS settings in appsettings.json:  
```json
"Cors": {
  "AllowedOrigins": [ "http://localhost", "http://localhost:4200" ],
  "AllowedMethods": [ "OPTIONS", "HEAD", "POST", "PUT", "DELETE", "GET" ],
  "AllowedHeaders": [
    "Content-Type",
    "Authorization",
    "Accept",
    "User-Agent",
    "Cache-Control",
    "Set-Cookie",
    "x-signalr-user-agent",
    "x-requested-with"
  ]
}
```
Specify everything that you would like to allow.  
When running in development it will allow anything but in production it will use the values here.  
Remember that CORS is supposed to be strict , so only allow what is needed.  

### Serilog settings in appsettings.json:  
Make sure you have appropriate settings for Serilog in appsettings.json.
* Serilog:MinimumLevel  
* Serilog:WriteTo:1:Args:Path (Where 1 represents the File-logger)  
Add any other settings that you would like to have. Loki etc  

### launchSettings.json
This API is using the WebApplicationBuilder to setup the host. It is adjusted for NET 9 and the new replacement for Swagger using OpenApi and Scalar. Make sure you have following in your application to enable browsing with Scalar:  
In Properties/launchSettings.json ensure that you have the following:  
```json
{
  "profiles": {
    "[YourApiName]": {
      ...
      "launchBrowser": true,
      "launchUrl": "scalar/v1",
      ...
    }
  }
}
```
### Other things to consider:
* Fill in all the sections marked with TODO  
* Add some kind of endpoints for the application to be able to respond to requests. Minimal Endpoints or FastEndpoints are recommended. In the template you will find a sample endpoint extension in Application/Endpoints/EndpointExtension  
* There is a sample of an endpoint outcommented in Program.cs showing how to use a minimal endpoint with metrics, sample includes a counter and a gauge. The metrics are user defined in a sample class named RequestCounterMetrics in the nuget package Common.K8s.Monitoring but you could of course do your own conters and gauges according to OpenTelemetry as well.  
* Add some kind of domain to the application. Sample is added under Domain, if you like to use MediatR to transport your requests into the domain is ok, otherwise use a domain service to do it. Remember to keep your business logic inside the domain.  
* Add an EF Core context to the application. Sample is located in Infrastructure/Data.  
* Use DbContext, EntityTypeConfiguration, Interceptors and Designtime classes like in the example, don't use old techniques!
* Expose your data section through a service, this could follow Repository pattern or even GenericRepository pattern.  
* Finally, run add-migration and update-database to create the database.
