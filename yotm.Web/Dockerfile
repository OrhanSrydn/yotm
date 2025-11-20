# Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyalarını kopyala
COPY ["yotm.Web/yotm.Web.csproj", "yotm.Web/"]

# Bağımlılıkları restore et
RUN dotnet restore "yotm.Web/yotm.Web.csproj"

# Tüm kaynak kodları kopyala
COPY . .

# Build
WORKDIR "/src/yotm.Web"
RUN dotnet build "yotm.Web.csproj" -c Release -o /app/build

# Publish aşaması
FROM build AS publish
RUN dotnet publish "yotm.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime aşaması
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "yotm.Web.dll"]
