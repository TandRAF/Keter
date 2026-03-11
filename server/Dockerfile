FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 1. Copiază și restaurează
COPY ["server.csproj", "./"]
RUN dotnet restore "server.csproj"

# 2. Copiază tot restul și publică în folderul /app/publish
COPY . ./
RUN dotnet publish "server.csproj" -c Release -o /app/publish

# Imaginea de rulare
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# 3. ALINIEREA: Copiem din /app/publish (unde am salvat mai sus) în folderul curent (.)
COPY --from=build /app/publish .

# 4. SCHIMBARE: Folosim server.dll (numele proiectului tău)
ENTRYPOINT ["dotnet", "server.dll"]