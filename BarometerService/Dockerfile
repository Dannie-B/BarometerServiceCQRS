FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
RUN apt-get update -y && apt-get install -y nano
COPY ./BarometerService/bin/publish .
ENTRYPOINT ["dotnet", "BarometerService.dll"]