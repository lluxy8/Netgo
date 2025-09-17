FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="$PATH:/root/.dotnet/tools"
ENV ConnectionStrings__MasterDb="Server=netgo.masterDb;Database=NetgoDb1;User Id=sa;Password=Strong!Passw0d;TrustServerCertificate=True"
ENV ConnectionStrings__IdentityDb="Server=netgo.identityDb;Database=NetgoDb2;User Id=sa;Password=Strong!Passw0d;TrustServerCertificate=True"

COPY ["Netgo.API/Netgo.API.csproj", "Netgo.API/"]
COPY ["Netgo.Persistence/Netgo.Persistence.csproj", "Netgo.Persistence/"]
COPY ["Netgo.Identity/Netgo.Identity.csproj", "Netgo.Identity/"]
COPY ["Netgo.Application/Netgo.Application.csproj", "Netgo.Application/"]
COPY ["Netgo.Domain/Netgo.Domain.csproj", "Netgo.Domain/"]
COPY ["Netgo.Infrastructure/Netgo.Infrastructure.csproj", "Netgo.Infrastructure/"]

RUN dotnet restore "Netgo.API/Netgo.API.csproj"

COPY . .

WORKDIR "/app/Netgo.API"

ENTRYPOINT ["/bin/bash", "-c", "dotnet ef database update --project /app/Netgo.Persistence/Netgo.Persistence.csproj --startup-project /app/Netgo.API/Netgo.API.csproj --context NetgoDbContext && dotnet ef database update --project /app/Netgo.Identity/Netgo.Identity.csproj --startup-project /app/Netgo.API/Netgo.API.csproj --context NetgoIdentityDbContext"]

# To start or update DBs

# docker build -f Netgo.Migrator.Dockerfile -t netgo-migrator . 
# docker run --rm --network dockercompose15106419983109714426_default netgo-migrator
