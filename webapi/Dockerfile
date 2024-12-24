# Use the ASP.NET Core runtime as the base image (Linux-based)
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000

# Use the SDK image to build the application (Linux-based)
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the project file to the container's /src directory
COPY webapi.csproj ./  # Make sure the correct path is used for your project file
RUN dotnet restore "webapi.csproj"  # Restore project dependencies

# Copy the rest of the application code into the container
COPY . . 

# Set working directory to the folder containing the project file
WORKDIR /src

# Build the project in Release mode
RUN dotnet build "webapi.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "webapi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image, set the entry point for the application (Linux-based)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .  # Copy the published output from the publish step
ENTRYPOINT ["dotnet", "webapi.dll"]
