FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Proto.Core.Presentation/Proto.Core.Presentation.csproj", "Proto.Core.Presentation/"]
COPY ["Proto.Common.Presentation/Proto.Common.Presentation.csproj", "Proto.Common.Presentation/"]
COPY ["Proto.Common.Application/Proto.Common.Application.csproj", "Proto.Common.Application/"]
COPY ["Proto.Core.Application/Proto.Core.Application.csproj", "Proto.Core.Application/"]
COPY ["Proto.Core.Domain/Proto.Core.Domain.csproj", "Proto.Core.Domain/"]
COPY ["Proto.Common.Domain/Proto.Common.Domain.csproj", "Proto.Common.Domain/"]
COPY ["Proto.Core.Infrastructure/Proto.Core.Infrastructure.csproj", "Proto.Core.Infrastructure/"]
COPY ["Proto.Common.Infrastructure/Proto.Common.Infrastructure.csproj", "Proto.Common.Infrastructure/"]
COPY ["Proto.Core.IoC/Proto.Core.IoC.csproj", "Proto.Core.IoC/"]

RUN dotnet restore "./Proto.Core.Presentation/Proto.Core.Presentation.csproj"
COPY . .

WORKDIR "/src/Proto.Core.Presentation"
RUN dotnet build "./Proto.Core.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Proto.Core.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Proto.Core.Presentation.dll"]
