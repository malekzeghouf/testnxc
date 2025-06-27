# Stage 1: Build the application
# Use the .NET SDK 9.0 image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the solution file first.
COPY ["./Neoledge.Elise.NxC.sln", "."]

# Copy all project files (*.csproj) from their respective directories.
# Add the newly identified missing projects here:
COPY ["./src/api/Neoledge.NxC.Api/Neoledge.NxC.Api.csproj", "src/api/Neoledge.NxC.Api/"]
COPY ["./src/aspire/Neoledge.NxC.ServiceDefaults/Neoledge.NxC.ServiceDefaults.csproj", "src/aspire/Neoledge.NxC.ServiceDefaults/"]
COPY ["./src/service/Neoledge.NxC.Service.NxC/Neoledge.NxC.Service.NxC.csproj", "src/service/Neoledge.NxC.Service.NxC/"]
COPY ["./src/db/Neoledge.NxC.Database/Neoledge.NxC.Database.csproj", "src/db/Neoledge.NxC.Database/"]
COPY ["./src/model/Neoledge.Nxc.Domain/Neoledge.Nxc.Domain.csproj", "src/model/Neoledge.Nxc.Domain/"]
COPY ["./src/db/Neoledge.NxC.Repository/Neoledge.NxC.Repository.csproj", "src/db/Neoledge.NxC.Repository/"]
COPY ["./src/service/Neoledge.NxC.Service.Certificate/Neoledge.NxC.Service.Certificate.csproj", "src/service/Neoledge.NxC.Service.Certificate/"]
COPY ["./src/service/Neoledge.NxC.Service.Cryptography/Neoledge.NxC.Service.Cryptography.csproj", "src/service/Neoledge.NxC.Service.Cryptography/"]
COPY ["./src/service/Neoledge.NxC.Service.MinoStorage/Neoledge.NxC.Service.MinoStorage.csproj", "src/service/Neoledge.NxC.Service.MinoStorage/"]

# NEWLY ADDED PROJECT COPY COMMANDS:
COPY ["./src/aspire/Neoledge.NxC.AppHost/Neoledge.NxC.AppHost.csproj", "src/aspire/Neoledge.NxC.AppHost/"]
COPY ["./src/api/Neoledge.NxC.Client.Api/Neoledge.NxC.Client.Api.csproj", "src/api/Neoledge.NxC.Client.Api/"]
COPY ["./src/db/Neoledge.NxC.DatabaseMigrationService/Neoledge.NxC.DatabaseMigrationService.csproj", "src/db/Neoledge.NxC.DatabaseMigrationService/"]
COPY ["./src/aspire/Neoledge.NxC.Aspire.Minio.Hosting/Neoledge.NxC.Aspire.Minio.Hosting.csproj", "src/aspire/Neoledge.NxC.Aspire.Minio.Hosting/"]
COPY ["./src/service/Neoledge.Nxc.Service.ApiConnector/Neoledge.Nxc.Service.ApiConnector.csproj", "src/service/Neoledge.Nxc.Service.ApiConnector/"]
COPY ["./src/service/Neoledge.NxC.Service.Client/Neoledge.NxC.Service.Client.csproj", "src/service/Neoledge.NxC.Service.Client/"]

# Restore NuGet packages for the entire solution.
RUN dotnet restore "./Neoledge.Elise.NxC.sln"

# Copy the rest of your source code.
COPY . .

# Set the working directory to the specific API project folder for build/publish.
WORKDIR "/src/src/api/Neoledge.NxC.Api"

# Build the specific API project.
RUN dotnet build "./Neoledge.NxC.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 2: Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/src/api/Neoledge.NxC.Api"
RUN dotnet publish "./Neoledge.NxC.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 3: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Neoledge.NxC.Api.dll"]