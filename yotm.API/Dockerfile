# Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyalarını kopyala
COPY ["yotm.API/yotm.API.csproj", "yotm.API/"]
COPY ["yotm.Core/yotm.Core.csproj", "yotm.Core/"]
COPY ["yotm.Insfrastructure/yotm.Insfrastructure.csproj", "yotm.Insfrastructure/"]

# Bağımlılıkları restore et
RUN dotnet restore "yotm.API/yotm.API.csproj"

# Tüm kaynak kodları kopyala
COPY . .

# Build
WORKDIR "/src/yotm.API"
RUN dotnet build "yotm.API.csproj" -c Release -o /app/build

# Publish aşaması
FROM build AS publish
RUN dotnet publish "yotm.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime aşaması
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "yotm.API.dll"]
