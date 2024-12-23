# Use the ASP.NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
# Expose port 80 (or any other port for your service) to the container
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["webapi/webapi.csproj", "webapi/"]
RUN dotnet restore "webapi/webapi.csproj"
COPY . . 
WORKDIR "/src/webapi"
RUN dotnet build "webapi.csproj" -c Release -o /app/build

# Publish the application in release mode
FROM build AS publish
RUN dotnet publish "webapi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image, set the entry point for the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Ensure the app listens on the correct port (defaulting to 5000 if not set)
ENTRYPOINT ["dotnet", "webapi.dll"]
