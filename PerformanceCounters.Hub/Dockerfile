FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
# Copy built artifacts from build environment
COPY --from=build-env /App/out .
# Copy environment variables from build environment to runtime environment
ENV DOCKER_CONNECTION_STRING="Server=host.docker.internal;Database=PerformanceCountersHub;User=PerformanceCountersHubUser;Password=12345;TrustServerCertificate=True;"
# Set entrypoint
ENTRYPOINT ["dotnet", "PerformanceCounters.Hub.dll"]
