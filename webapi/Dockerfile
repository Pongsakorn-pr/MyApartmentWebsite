# Use the ASP.NET Core runtime as the base image (Linux-based)
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5000

# Use the SDK image to build the application (Linux-based)
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["webapi.csproj", "webapi/"]
RUN dotnet restore "webapi/webapi.csproj"
COPY . . 
WORKDIR "/src/webapi"
RUN dotnet build "webapi.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "webapi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image, set the entry point for the application (Linux-based)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "webapi.dll"]
