FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copy the project file and solution file (if you have one in the root of your build context)
# You might need to adjust this if your solution file is one level up.
# Assuming the Dockerfile is in the same directory as Neoledge.NxC.Api.csproj
COPY ["./Neoledge.NxC.Api.csproj", "./"] 
# If you have a solution file, add it here:
# COPY ["./YourSolutionName.sln", "./"] 

RUN dotnet restore "./Neoledge.NxC.Api.csproj"
# The 'COPY . .' should happen AFTER restore, to leverage caching and avoid re-restoring on code changes.
# It copies the entire build context into /src
COPY . . 

# The WORKDIR is already /src from Step 8. No need for Step 12 to change to /src again if already there.
# If you're building a solution with multiple projects, you might want to adjust the WORKDIR and COPY strategy.
# For a single project API, this is fine.

# Change the build path to directly reference the .csproj in /src
RUN dotnet build "./Neoledge.NxC.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# Change the publish path to directly reference the .csproj in /src
RUN dotnet publish "./Neoledge.NxC.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Neoledge.NxC.Api.dll"]