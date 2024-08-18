FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Basket.API/Basket.API.csproj", "Basket.API/"]
COPY ["Basket.Application/Basket.Application.csproj", "Basket.Application/"]
COPY ["Basket.Core/Basket.Core.csproj", "Basket.Core/"]
COPY ["Basket.Infrastructure/Basket.Infrastructure.csproj", "Basket.Infrastructure/"]
COPY ["EventBus.Messages/EventBus.Messages.csproj", "EventBus.Messages/"]

RUN dotnet restore "Basket.API/Basket.API.csproj"
COPY . .
WORKDIR "/src/Basket.API"
RUN dotnet build "Basket.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Basket.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.API.dll"]
