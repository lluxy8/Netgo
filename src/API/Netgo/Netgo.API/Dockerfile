# Hata ayıklama kapsayıcınızı özelleştirme ve Visual Studio’nun daha hızlı hata ayıklama için görüntülerinizi derlemek üzere bu Dockerfile'ı nasıl kullandığı hakkında bilgi edinmek için https://aka.ms/customizecontainer sayfasına bakın.

# Bu aşama, VS'den hızlı modda çalıştırıldığında kullanılır (Hata ayıklama yapılandırması için varsayılan olarak ayarlıdır)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Bu aşama, hizmet projesini oluşturmak için kullanılır
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Netgo.API/Netgo.API.csproj", "Netgo.API/"]
COPY ["Netgo.Application/Netgo.Application.csproj", "Netgo.Application/"]
COPY ["Netgo.Domain/Netgo.Domain.csproj", "Netgo.Domain/"]
COPY ["Netgo.Infrastructure/Netgo.Infrastructure.csproj", "Netgo.Infrastructure/"]
COPY ["Netgo.Persistence/Netgo.Persistence.csproj", "Netgo.Persistence/"]
RUN dotnet restore "./Netgo.API/Netgo.API.csproj"
COPY . .
WORKDIR "/src/Netgo.API"
RUN dotnet build "./Netgo.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Bu aşama, son aşamaya kopyalanacak hizmet projesini yayımlamak için kullanılır
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Netgo.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Bu aşama üretimde veya VS'den normal modda çalıştırıldığında kullanılır (Hata Ayıklama yapılandırması kullanılmazken varsayılan olarak ayarlıdır)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Netgo.API.dll"]