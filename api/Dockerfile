# Stage 1: Build the application (Updated to 9.0)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Keter.sln", "./"]
COPY ["Keter.Domain/Keter.Domain.csproj", "Keter.Domain/"]
COPY ["Keter.Api/Keter.Api.csproj", "Keter.Api/"]
RUN dotnet restore "Keter.sln"

COPY . .

WORKDIR "/src/Keter.Api"
RUN dotnet publish "Keter.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Create the final production image (Updated to 9.0)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "Keter.Api.dll"]