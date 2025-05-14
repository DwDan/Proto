FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Proto.Core.Presentation/Proto.Core.Presentation.csproj", "Proto.Core.Presentation/"]
COPY ["Demo.Common.Presentation/Demo.Common.Presentation.csproj", "Demo.Common.Presentation/"]
COPY ["Demo.Common.Application/Demo.Common.Application.csproj", "Demo.Common.Application/"]
COPY ["Proto.Core.Application/Proto.Core.Application.csproj", "Proto.Core.Application/"]
COPY ["Proto.Core.Domain/Proto.Core.Domain.csproj", "Proto.Core.Domain/"]
COPY ["Demo.Common.Domain/Demo.Common.Domain.csproj", "Demo.Common.Domain/"]
COPY ["Proto.Core.Infrastructure/Proto.Core.Infrastructure.csproj", "Proto.Core.Infrastructure/"]
COPY ["Demo.Common.Infrastructure/Demo.Common.Infrastructure.csproj", "Demo.Common.Infrastructure/"]
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
